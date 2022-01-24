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
    public class AcademyController : ControllerBase
    {
        private readonly IAcademyService academyService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;

        public AcademyController(IAcademyService academyService, IUriService uriService, IMapper mapper)
        {
            this.academyService = academyService;

            this.uriService = uriService;

            this.mapper = mapper;
        }

        [HttpPost(ApiRoutes.AcademyRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] AcademyRequest academyRequest)
        {
            var academy = mapper.Map<Academy>(academyRequest);

            var creationResult = await academyService.CreationAsync(academy);

            if (!creationResult.Succeded) return Conflict(creationResult);

            var parameter = new Dictionary<string, string>{
                { "{id}", creationResult.Data.Id}
            };

            var creationResponse = new CreationResponse<AcademyResponse>
            {
                Data = mapper.Map<AcademyResponse>(creationResult.Data),
                GetUri = uriService.GetModelUri(parameter, ApiRoutes.AcademyRoutes.Get),
                Succeded = true
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        
        [HttpGet(ApiRoutes.AcademyRoutes.Get)]
        public async Task<IActionResult> Get([FromQuery] AcademyQuery query)
        {
            var academy = await academyService.GetAsync(query);

            if (academy == null) return NoContent();

            var response = new Response<AcademyResponse>(mapper.Map<AcademyResponse>(academy));

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.AcademyRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
        {
            if (query == null) return BadRequest();
            
                var filter = mapper.Map<PaginationFilter>(query);

                var searchMode = filter.SearchValue != null;

                var academies = await academyService.GetAllAsync(filter);

                var academiesResponse = mapper.Map<List<AcademyResponse>>(academies);

                var pageResponse = new PageResponse<AcademyResponse>(academiesResponse);

                if (filter.PageNumber < 1 || filter.PageSize < 1)
                    return Ok(pageResponse);

                var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                                                            response: academiesResponse,
                                                                            uriService: uriService,
                                                                            path: ApiRoutes.AcademyRoutes.GetAll,
                                                                            searchMode: searchMode);

                return Ok(paginationResponse);
        }
    }
}
