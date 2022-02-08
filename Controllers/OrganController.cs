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
    public class OrganController : ControllerBase
    {
        private readonly IOrganService organService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;

        public OrganController(IOrganService organService, IUriService uriService, IMapper mapper)
        {
            this.organService = organService;

            this.uriService = uriService;

            this.mapper = mapper;
        }

        [HttpPost(ApiRoutes.OrganRoutes.Create)]
        public async Task<IActionResult> Create([FromBody]OrganRequest organRequest)
        {
            var organ = mapper.Map<Organ>(organRequest);

            var creationResult = await organService.CreateAsync(organ);

            if (!creationResult.Succeded) return Conflict(creationResult);

            var parameter = new Dictionary<string, string>{
                { "{id}", creationResult.Data.Id}
            };

            var creationResponse = new CreationResponse<OrganResponse>
            {
                Data = mapper.Map<OrganResponse>(creationResult.Data),
                GetUri = uriService.GetModelUri(parameter, ApiRoutes.OrganRoutes.Get),
                Succeded = true
            };

            return Created(creationResponse.GetUri, creationResponse);
        }               
    }
}
