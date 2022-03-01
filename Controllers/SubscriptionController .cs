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
    public class SubscriptionController  : ControllerBase
    {
        private readonly ISubscriptionService serice;

        private readonly IMapper mapper;

        private readonly IUriService uriService;


        public SubscriptionController (ISubscriptionService studantService, IMapper mapper, IUriService uriService)
        {
            this.serice = studantService;

            this.mapper = mapper;

            this.uriService = uriService;
        }
     
        [HttpGet(ApiRoutes.SubscriptionRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string studantId, [FromRoute] string formationId)
        {
            if (studantId is null || formationId is null) return BadRequest();

            var subscription = await serice.GetAsync(studantId, formationId);

            if (subscription == null) return BadRequest();

            var response = new Response<SubscriptionResponse>(mapper.Map<SubscriptionResponse>(subscription));

            return Ok(response);
        }               
    }
}
