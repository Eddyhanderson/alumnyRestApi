using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alumni.Contracts.V1;
using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Requests.Queries;
using alumni.Contracts.V1.Responses;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using Alumni.Helpers.PaginationHelpers;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace alumni.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService questionService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;


        public QuestionController(IQuestionService questionService, IMapper mapper, IUriService uriService)
        {
            this.questionService = questionService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [Authorize(Roles = Constants.UserContansts.StudantRole)]
        [HttpPost(ApiRoutes.QuestionRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] QuestionRequest questionRequest)
        {
            if (questionRequest == null) return BadRequest();

            string route = ApiRoutes.QuestionRoutes.Get;

            var question = mapper.Map<Question>(questionRequest);

            var creationResult = await questionService.CreateAsync(question);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<QuestionResponse>
            {
                Data = mapper.Map<QuestionResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.QuestionRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query, [FromQuery] QuestionQuery questionQuery)
        {
            if (query == null || questionQuery == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(query);

            var searchMode = filter.SearchValue != null;

            var pageResult = await questionService.GetQuestionsAsync(filter, questionQuery);

            var pageResponse = new PageResponse<QuestionResponse>
            {
                Data = mapper.Map<IEnumerable<Question>, IEnumerable<QuestionResponse>>(pageResult.Data),
                TotalElements = pageResult.TotalElements
            };

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: pageResponse.Data,
                                    uriService: uriService,
                                    path: ApiRoutes.LessonRoutes.GetAll,
                                    searchMode: searchMode);

            paginationResponse.TotalElements = pageResult.TotalElements;

            for (int i = 0; i < paginationResponse.Data.Count(); i++)
            {
                var question = paginationResponse.Data.ElementAt(i);
                question.StudantAnswerQnt = await questionService.GetStudantAnswerQntAsync(question.Id);
                question.TeacherAnswerQnt = await questionService.GetTeacherAnswerQntAsync(question.Id);
                question.CommentsQnt = await questionService.GetCommentsQntAsync(question.Id);

            }

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.QuestionRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            if (Id != null)
            {
                var question = await questionService.GetQuestionAsync(Id);

                if (question == null) return NotFound();

                var response = new Response<QuestionResponse>(mapper.Map<QuestionResponse>(question));

                var questionResponse = response.Data;
                questionResponse.StudantAnswerQnt = await questionService.GetStudantAnswerQntAsync(question.Id);
                questionResponse.TeacherAnswerQnt = await questionService.GetTeacherAnswerQntAsync(question.Id);
                questionResponse.CommentsQnt = await questionService.GetCommentsQntAsync(question.Id);

                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPatch(ApiRoutes.QuestionRoutes.Patch)]
        public async Task<IActionResult> Patch([FromRoute] string Id, [FromBody] JsonPatchDocument<Question> patchDoc)
        {
            if (Id == null) return BadRequest();

            var question = await questionService.GetQuestionAsync(Id);

            if (question == null) return BadRequest();

            patchDoc.ApplyTo(question);

            await questionService.PatchQuestionAsync(question);

            var questionResponse = mapper.Map<QuestionResponse>(question);

            return Ok(new Response<QuestionResponse>(questionResponse));
        }

    }
}
