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
    public class FormationEventController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IFormationEventService service;

        private readonly IUriService uriService;
        public FormationEventController(IMapper mapper, IFormationEventService service, IUriService uriService)
        {
            this.mapper = mapper;

            this.service = service;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.FormationEventRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] FormationEventRequest request)
        {
            if (request is null) return BadRequest();
            var formation = mapper.Map<FormationEvent>(request);

            var creationResult = await service.CreateAsync(formation);

            if (!creationResult.Succeded) return Conflict(creationResult);

            var parameter = new Dictionary<string, string>{
                    { "{id}", creationResult.Data.Id}
                };

            var creationResponse = new CreationResponse<FormationEventResponse>
            {
                Data = mapper.Map<FormationEventResponse>(creationResult.Data),
                GetUri = uriService.GetModelUri(parameter, ApiRoutes.FormationEventRoutes.Get),
                Succeded = true
            };

            return Created(creationResponse.GetUri, creationResponse);
        }
    }
}