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
    public class FormationRequestController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IFormationRequestService service;

        private readonly IUriService uriService;
        public FormationRequestController(IMapper mapper, IFormationRequestService service, IUriService uriService)
        {
            this.mapper = mapper;

            this.service = service;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.FormationRequestRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] FormationRequestRequest request)
        {
            if (request is null) return BadRequest();
            var formationRequest = mapper.Map<FormationRequest>(request);

            var creationResult = await service.CreateAsync(formationRequest);

            if (!creationResult.Succeded) return Conflict(creationResult);

            var parameter = new Dictionary<string, string>{
                    { "{id}", creationResult.Data.Id}
                };

            var creationResponse = new CreationResponse<FormationRequestResponse>
            {
                Data = mapper.Map<FormationRequestResponse>(creationResult.Data),
                GetUri = uriService.GetModelUri(parameter, ApiRoutes.FormationRequestRoutes.Get),
                Succeded = true
            };

            return Created(creationResponse.GetUri, creationResponse);
        }


        [HttpGet(ApiRoutes.FormationRequestRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string studantId, [FromRoute] string formationId)
        {
            if (studantId is null || formationId is null) return BadRequest();

            var request = await service.GetFormationRequestAsync(studantId, formationId);

            if (request == null) return NoContent();

            var response = new Response<FormationRequestResponse>(mapper.Map<FormationRequestResponse>(request));

            SetResponseData(request, response.Data);

            return Ok(response);
        }

        private FormationRequestResponse SetResponseData(FormationRequest request, FormationRequestResponse response)
        {
            response.FormationStart = request.Formation.FormationEvents[0].Start;
            
            return response;
        }
    }
}