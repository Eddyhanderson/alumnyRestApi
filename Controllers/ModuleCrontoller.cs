using System.Collections.Generic;
using System.Threading.Tasks;
using alumni.Contracts.V1;
using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Requests.Queries;
using alumni.Contracts.V1.Responses;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers.PaginationHelpers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace alumni.Controllers
{
    public class ModuleController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IModuleService service;

        private readonly IUriService uriService;
        public ModuleController(IMapper mapper, IModuleService service, IUriService uriService)
        {
            this.mapper = mapper;

            this.service = service;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.ModuleRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] CreateModuleRequest moduleRequest)
        {
            if (moduleRequest is null) return BadRequest();
            var module = mapper.Map<Module>(moduleRequest);

            var creationResult = await service.CreateAsync(module);

            if (!creationResult.Succeded) return Conflict(creationResult);

            var parameter = new Dictionary<string, string>{
                    { "{id}", creationResult.Data.Id}
                };

            var creationResponse = new CreationResponse<ModuleResponse>
            {
                Data = mapper.Map<ModuleResponse>(creationResult.Data),
                GetUri = uriService.GetModelUri(parameter, ApiRoutes.ModuleRoutes.Get),
                Succeded = true
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.ModuleRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery pagQuery, [FromQuery] ModuleQuery query)
        {
            var filter = mapper.Map<PaginationFilter>(pagQuery);

            var searchMode = filter.SearchValue != null;

            var pageResult = await service.GetModulesAsync(filter, query);

            var pageResponse = new PageResponse<ModuleResponse>
            {
                Data = mapper.Map<IEnumerable<Module>, IEnumerable<ModuleResponse>>(pageResult.Data),
                TotalElements = pageResult.TotalElements
            };

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: pageResponse.Data,
                                    uriService: uriService,
                                    path: ApiRoutes.FormationRoutes.GetAll,
                                    searchMode: searchMode);

            paginationResponse.TotalElements = pageResponse.TotalElements;

            return Ok(paginationResponse);
        }


        [HttpGet(ApiRoutes.ModuleRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            if (id is null) return BadRequest();

            var module = await service.GetModuleAsync(id);

            if (module == null) return NotFound();

            var response = new Response<ModuleResponse>(mapper.Map<ModuleResponse>(module));

            return Ok(response);
        }


    }

}