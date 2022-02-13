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
using Microsoft.AspNetCore.Mvc;


namespace alumni.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class DisciplineController : ControllerBase
    {
        private readonly IDisciplineService disciplineService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;


        public DisciplineController(IDisciplineService disciplineService, IMapper mapper, IUriService uriService)
        {
            this.disciplineService = disciplineService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.DisciplineRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] DisciplineRequest disciplineRequest)
        {
            if (disciplineRequest == null) return BadRequest();

            string route = ApiRoutes.DisciplineRoutes.Get;

            var discipline = mapper.Map<Discipline>(disciplineRequest);

            var creationResult = await disciplineService.CreateAsync(discipline);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<DisciplineResponse>
            {
                Data = mapper.Map<DisciplineResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.DisciplineRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
        {
            if (query == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(query);

            var searchMode = filter.SearchValue != null;

            var disciplines = await disciplineService.GetDisciplinesAsync(filter);

            var disciplinesResponse = mapper.Map<List<DisciplineResponse>>(disciplines);

            var pageResponse = new PageResponse<DisciplineResponse>(disciplinesResponse);

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: disciplinesResponse,
                                    uriService: uriService,
                                    path: ApiRoutes.DisciplineRoutes.GetAll,
                                    searchMode: searchMode);

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.DisciplineRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            if (Id != null)
            {
                var discipline = await disciplineService.GetDisciplineAsync(Id);

                if (discipline == null) return BadRequest();

                var response = new Response<DisciplineResponse>(mapper.Map<DisciplineResponse>(discipline));

                return Ok(response);
            }

            return BadRequest();
        }
    
    }
}
