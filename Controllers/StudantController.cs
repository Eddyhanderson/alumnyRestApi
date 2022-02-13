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
    public class StudantController : ControllerBase
    {
        private readonly IStudantService studantService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;


        public StudantController(IStudantService studantService, IMapper mapper, IUriService uriService)
        {
            this.studantService = studantService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.StudantRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] StudantRequest studantRequest)
        {
            if (studantRequest == null) return BadRequest();

            string route = ApiRoutes.StudantRoutes.Get;

            var studant = mapper.Map<Studant>(studantRequest);

            var creationResult = await studantService.CreateAsync(studant);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<StudantResponse>
            {
                Data = mapper.Map<StudantResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.StudantRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
        {
            if (query == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(query);

            filter.UserId = HttpContext.GetUser();

            var searchMode = filter.SearchValue != null;

            var pageResult = await studantService.GetStudantsAsync(filter);

            var pageResponse = new PageResponse<StudantResponse>
            {
                Data = mapper.Map<IEnumerable<StudantResponse>>(pageResult.Data),
                TotalElements = pageResult.TotalElements
            };

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: pageResponse.Data,
                                    uriService: uriService,
                                    path: ApiRoutes.StudantRoutes.GetAll,
                                    searchMode: searchMode);

            paginationResponse.TotalElements = pageResult.TotalElements;

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.StudantRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            if (Id == null) return BadRequest();

            var studant = await studantService.GetStudantAsync(Id);

            if (studant == null) return BadRequest();

            var response = new Response<StudantResponse>(mapper.Map<StudantResponse>(studant));

            return Ok(response);
        }

    }
}
