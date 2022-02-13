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

    public class TeacherPlaceController : ControllerBase
    {
        private readonly ITeacherPlaceService teacherPlaceService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;


        public TeacherPlaceController(ITeacherPlaceService teacherPlaceService, IMapper mapper, IUriService uriService)
        {
            this.teacherPlaceService = teacherPlaceService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [Authorize(Roles = Constants.UserContansts.SchoolRole)]
        [HttpPost(ApiRoutes.TeacherPlaceRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] TeacherPlaceRequest teacherPlaceRequest)
        {
            if (teacherPlaceRequest == null) return BadRequest();

            string route = ApiRoutes.TeacherPlaceRoutes.Get;

            var teacherPlace = mapper.Map<TeacherPlace>(teacherPlaceRequest);

            var creationResult = await teacherPlaceService.CreateAsync(teacherPlace);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<TeacherPlaceResponse>
            {
                Data = mapper.Map<TeacherPlaceResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.TeacherPlaceRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery pagQuery, [FromQuery] TeacherPlaceQuery param)
        {
            var filter = mapper.Map<PaginationFilter>(pagQuery);

            var searchMode = filter.SearchValue != null;

            var teacherPlaces = await teacherPlaceService.GetTeacherPlacesAsync(filter, param);

            var teacherPlacesResponse = mapper.Map<List<TeacherPlaceResponse>>(teacherPlaces);

            var pageResponse = new PageResponse<TeacherPlaceResponse>(teacherPlacesResponse);


            for (int i = 0; i < pageResponse.Data.Count(); i++)
                await SetAnalytics(pageResponse.Data.ElementAt(i));

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: teacherPlacesResponse,
                                    uriService: uriService,
                                    path: ApiRoutes.TeacherPlaceRoutes.GetAll,
                                    searchMode: searchMode);

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.TeacherPlaceRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            if (Id != null)
            {
                var teacherPlace = await teacherPlaceService.GetTeacherPlaceAsync(Id);

                if (teacherPlace == null) return BadRequest();

                var response = new Response<TeacherPlaceResponse>(mapper.Map<TeacherPlaceResponse>(teacherPlace));

                await SetAnalytics(response.Data);

                return Ok(response);
            }

            return BadRequest();
        }

        private async Task SetAnalytics(TeacherPlaceResponse teacherPlace)
        {
            teacherPlace.AnswerCount = await teacherPlaceService.AnswerCountAsync(teacherPlace.Id);
            teacherPlace.LessonsCount = await teacherPlaceService.LessonsCountAsync(teacherPlace.Id);
            teacherPlace.SolvedQuestionCount = await teacherPlaceService.SolvedQuestionCountAsync(teacherPlace.Id);
            teacherPlace.StudantAnswerCount = await teacherPlaceService.StudantAnswerCountAsync(teacherPlace.Id);
            teacherPlace.QuestionsCount = await teacherPlaceService.QuestionsCountAsync(teacherPlace.Id);
            teacherPlace.StudantsCount = await teacherPlaceService.StudantsCountAsync(teacherPlace.Id);
            teacherPlace.TeacherAnswerCount = await teacherPlaceService.TeacherAnswerCountAsync(teacherPlace.Id);
            teacherPlace.TopicCount = await teacherPlaceService.TopicCountAsync(teacherPlace.Id);

        }

    }
}
