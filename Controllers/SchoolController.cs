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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace alumni.Controllers
{
    [ApiController]
    public class SchoolController : Controller
    {
        private readonly ISchoolService schoolService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;

        public SchoolController(ISchoolService schoolService, IMapper mapper, IUriService uriService)
        {
            this.schoolService = schoolService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.SchoolRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] SchoolRequest schoolRequest)
        {
            if (schoolRequest == null) return BadRequest();

            string route = ApiRoutes.SchoolRoutes.Get;

            var school = mapper.Map<School>(schoolRequest);

            var creationResult = await schoolService.CreateAsync(school);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{Id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<SchoolResponse>
            {
                Data = mapper.Map<SchoolResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.SchoolRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query, [FromQuery] SchoolQuery param)
        {
            if (query == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(query);

            var searchMode = filter.SearchValue != null;

            var schools = await schoolService.GetSchoolsAsync(filter, param);

            var schoolsResponse = mapper.Map<List<SchoolResponse>>(schools);

            var pageResponse = new PageResponse<SchoolResponse>(schoolsResponse);

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: schoolsResponse,
                                    uriService: uriService,
                                    path: ApiRoutes.SchoolRoutes.GetAll,
                                    searchMode: searchMode);

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.SchoolRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            if (Id != null)
            {
                var school = await schoolService.GetSchoolAsync(Id);

                if (school == null) return BadRequest();

                var schoolResponse = mapper.Map<SchoolResponse>(school);

                var response = new Response<SchoolResponse>(schoolResponse);

                return Ok(response);
            }

            return BadRequest();
        }               
    }
}
