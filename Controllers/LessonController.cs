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
using Microsoft.AspNetCore.Mvc;

namespace alumni.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService lessonService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;


        public LessonController(ILessonService lessonService,
            IMapper mapper, IUriService uriService)
        {
            this.lessonService = lessonService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [Authorize(Roles = Constants.UserContansts.SchoolRole)]
        [HttpPost(ApiRoutes.LessonRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] LessonRequest lessonRequest)
        {
            if (lessonRequest == null) return BadRequest();

            string route = ApiRoutes.LessonRoutes.Get;

            var lesson = mapper.Map<Lesson>(lessonRequest);

            var creationResult = await lessonService.CreateAsync(lesson);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<LessonResponse>
            {
                Data = mapper.Map<LessonResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.LessonRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query, [FromQuery] LessonQuery lessonQuery)
        {
            if (query == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(query);

            var searchMode = filter.SearchValue != null;

            var pageResult = await lessonService.GetLessonsAsync(filter, lessonQuery);            

            var pageResponse = new PageResponse<LessonResponse> { 
                Data = mapper.Map<IEnumerable<Lesson>, IEnumerable<LessonResponse>>(pageResult.Data),
                TotalElements = pageResult.TotalElements
            }; 

            for(int i = 0; i < pageResponse.Data.Count(); i++)
            {
                await SetAnalytics(pageResponse.Data.ElementAt(i));
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
        
        [HttpGet(ApiRoutes.LessonRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            if (Id == null) return BadRequest();

            var lesson = await lessonService.GetLessonAsync(Id);

            if (lesson == null) return BadRequest();

            var response = new Response<LessonResponse>(mapper.Map<LessonResponse>(lesson));

            return Ok(response);
        }

        private async Task SetAnalytics(LessonResponse lesson){
            lesson.QuestionCount = await lessonService.QuestionCountAsync(lesson.Id);;
        }
    }
}