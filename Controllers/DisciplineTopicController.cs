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
using Microsoft.AspNetCore.Server.HttpSys;

namespace alumni.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class DisciplineTopicController : ControllerBase
    {
        private readonly IDisciplineTopicService disciplineTopicService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;


        public DisciplineTopicController(IDisciplineTopicService disciplineTopicService,
            IMapper mapper, IUriService uriService)
        {
            this.disciplineTopicService = disciplineTopicService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.DisciplineTopicRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] DisciplineTopicRequest disciplineTopicRequest)
        {
            if (disciplineTopicRequest == null) return BadRequest();

            string route = ApiRoutes.DisciplineTopicRoutes.Get;

            var disciplineTopic = mapper.Map<DisciplineTopic>(disciplineTopicRequest);

            var creationResult = await disciplineTopicService.CreateAsync(disciplineTopic);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{Id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<DisciplineTopicResponse>
            {
                Data = mapper.Map<DisciplineTopicResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.DisciplineTopicRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query, [FromQuery] DisciplineTopicQuery disciplineTopicQuery)
        {
            if (query == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(query);

            var searchMode = filter.SearchValue != null;

            var disciplineTopics = await disciplineTopicService.GetDisciplineTopicsAsync(filter, disciplineTopicQuery);

            var disciplinesTopicResponse = mapper.Map<List<DisciplineTopicResponse>>(disciplineTopics);

            var pageResponse = new PageResponse<DisciplineTopicResponse>(disciplinesTopicResponse);

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: disciplinesTopicResponse,
                                    uriService: uriService,
                                    path: ApiRoutes.DisciplineTopicRoutes.GetAll,
                                    searchMode: searchMode);

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.DisciplineTopicRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            if (Id == null) return BadRequest();

            var disciplineTopic = await disciplineTopicService.GetDisciplineTopicAsync(Id);

            if (disciplineTopic == null) return BadRequest();

            var response = new Response<DisciplineTopicResponse>(mapper.Map<DisciplineTopicResponse>(disciplineTopic));

            return Ok(response);
        }
    }
}
