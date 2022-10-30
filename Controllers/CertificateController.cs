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
    public class CertificateController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICertificateService service;

        private readonly IUriService uriService;
        public CertificateController(IMapper mapper, ICertificateService service, IUriService uriService)
        {
            this.mapper = mapper;

            this.service = service;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.CertificateRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] CertificateRequest certificateRequest)
        {
            if (certificateRequest is null) return BadRequest();

            var certificate = mapper.Map<Certificate>(certificateRequest);

            var creationResult = await service.CreateAsync(certificate);

            if (!creationResult.Succeded) return Conflict(creationResult);

            var parameter = new Dictionary<string, string>{
                    { "{id}", creationResult.Data.Id}
                };

            var creationResponse = new CreationResponse<CertificateResponse>
            {
                Data = mapper.Map<CertificateResponse>(creationResult.Data),
                GetUri = uriService.GetModelUri(parameter, ApiRoutes.ModuleRoutes.Get),
                Succeded = true
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.CertificateRoutes.GetBySubscription)]
        public async Task<IActionResult> Get([FromRoute] string subscriptionId)
        {
            if (subscriptionId is null) return BadRequest();

            var certificate = await service.GetCertificateBySubscriptionAsync(subscriptionId);

            if (certificate == null) return NotFound();

            var response = new Response<CertificateResponse>(mapper.Map<CertificateResponse>(certificate));

            return Ok(response);
        }


    }

}