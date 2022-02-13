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
    public class FormationController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IFormationService service;

        private readonly IUriService uriService;
        public FormationController(IMapper mapper, IFormationService service, IUriService uriService)
        {
            this.mapper = mapper;

            this.service = service;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.FormationRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] CreateFormationRequest formationRequest)
        {
            var formation = mapper.Map<Formation>(formationRequest);

            var creationResult = await service.CreateAsync(formation);

            if (!creationResult.Succeded) return Conflict(creationResult);

            var parameter = new Dictionary<string, string>{
                    { "{id}", creationResult.Data.Id}
                };

            var creationResponse = new CreationResponse<FormationCreationResponse>
            {
                Data = mapper.Map<FormationCreationResponse>(creationResult.Data),
                GetUri = uriService.GetModelUri(parameter, ApiRoutes.FormationRoutes.Get),
                Succeded = true
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

    }
}