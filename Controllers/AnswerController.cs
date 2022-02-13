using alumni.Contracts.V1;
using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Requests.Queries;
using alumni.Contracts.V1.Responses;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using Alumni.Helpers.PaginationHelpers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Controllers
{
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService answerService;

        private readonly IUserService userService;

        private readonly IQuestionService questionService;

        private readonly ITeacherService teacherService;

        private readonly ICommentService commentService;

        private readonly UserManager<User> userManager;

        private readonly IMapper mapper;

        private readonly IUriService uriService;

        public AnswerController(IAnswerService answerService,
            IMapper mapper, IUriService uriService,
            IUserService userService, UserManager<User> userManager,
            IQuestionService questionService, ITeacherService teacherService,
            ICommentService commentService)
        {
            this.answerService = answerService;

            this.mapper = mapper;

            this.uriService = uriService;

            this.userService = userService;

            this.userManager = userManager;

            this.questionService = questionService;

            this.teacherService = teacherService;

            this.commentService = commentService;
        }

        [HttpPost(ApiRoutes.AnswerRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] AnswerRequest answerRequest)
        {
            if (answerRequest == null) return BadRequest();

            string route = ApiRoutes.QuestionRoutes.Get;

            var answer = mapper.Map<Answer>(answerRequest);

            var creationResult = await answerService.CreateAsync(answer);

            if (!creationResult.Succeded) return Conflict();

            var isTeacher = HttpContext.GetRole().ToUpper() == Constants.UserContansts.SchoolRole.ToUpper();

            if (isTeacher)
            {
                var question = await questionService.GetQuestionAsync(creationResult.Data.QuestionId);

                var teacher = await teacherService.GetTeacherByLessonAsync(question.LessonId);

                // If the answer is from the teacher who created the lesson
                if (teacher.UserId == HttpContext.GetUser())
                {
                    if (QuestionSituations.Analyzing.ToString("g") == question.Situation)
                    {
                        JsonPatchDocument<Question> path = new JsonPatchDocument<Question>();
                        path.Replace(q => q.Situation, QuestionSituations.Opened.ToString("g"));
                        path.ApplyTo(question);

                        await questionService.PatchQuestionAsync(question);
                    }
                }
            }

            var parameter = new Dictionary<string, string>
            {
                {"{Id}",creationResult.Data.Id }
            };

            var creationResponse = new CreationResponse<AnswerResponse>
            {
                Data = mapper.Map<AnswerResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.AnswerRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            if (Id != null)
            {
                var answer = await answerService.GetAsync(Id);

                if (answer == null) return NotFound();

                var response = new Response<AnswerResponse>(mapper.Map<AnswerResponse>(answer));

                return Ok(response);
            }

            return BadRequest();
        }

        [HttpGet(ApiRoutes.AnswerRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query, [FromQuery] AnswerQuery answerQuery)
        {
            if (query == null || answerQuery == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(query);

            var searchMode = filter.SearchValue != null;

            var pageResult = await answerService.GetAllAsync(filter, answerQuery);

            var pageResponse = new PageResponse<AnswerResponse>
            {
                Data = mapper.Map<IEnumerable<Answer>, IEnumerable<AnswerResponse>>(pageResult.Data),
                TotalElements = pageResult.TotalElements
            };

            for (int i = 0; i < pageResponse.Data.Count(); i++)
            {
                var answer = pageResponse.Data.ElementAt(i);

                var user = await userManager.FindByIdAsync(answer.UserId);

                var isTeacher = await userManager.IsInRoleAsync(user, Constants.UserContansts.SchoolRole);

                if (isTeacher)
                {
                    var teacher = await userService.GetTeacherAsync(user.Id);

                    answer.UserCourse = teacher.Course.Name;
                    answer.UserAcademy = teacher.Academy.Name;
                    answer.UserAcademicLevel = teacher.AcademicLevel.Name;
                    answer.UserRole = Constants.UserContansts.SchoolRole;                    
                }
                else
                {
                    var studant = await userService.GetStudantAsync(user.Id);

                    /*answer.UserCourse = studant.Course.Name;
                    answer.UserAcademy = studant.Academy.Name;
                    answer.UserAcademicLevel = studant.AcademicLevel.Name;*/
                    answer.UserRole = Constants.UserContansts.StudantRole;                    
                }
            }

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: pageResponse.Data,
                                    uriService: uriService,
                                    path: ApiRoutes.LessonRoutes.GetAll,
                                    searchMode: searchMode);

            paginationResponse.TotalElements = pageResult.TotalElements;

            return Ok(paginationResponse);
        }

    }
}
