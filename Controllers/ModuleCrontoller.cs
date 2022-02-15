using System.Collections.Generic;
using System.Threading.Tasks;
using alumni.Contracts.V1;
using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Responses;
using alumni.Domain;
using alumni.IServices;
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

            var creationResponse = new CreationResponse<ModuleCreationResponse>
            {
                Data = mapper.Map<ModuleCreationResponse>(creationResult.Data),
                GetUri = uriService.GetModelUri(parameter, ApiRoutes.ModuleRoutes.Get),
                Succeded = true
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

    }
}