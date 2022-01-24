using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alumni.Contracts.V1;
using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Responses;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace alumni.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class BadgeInformationController : ControllerBase
    {
        private readonly IBadgeInformationService badgeInformationService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;


        public BadgeInformationController(IBadgeInformationService badgeInformationService, IMapper mapper, IUriService uriService)
        {
            this.badgeInformationService = badgeInformationService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.BadgeInformationRoutes.Create)]
        public async Task<IActionResult> Create()
        {
            string route = ApiRoutes.BadgeInformationRoutes.Get;

            BadgeInformation badgeInformationRequest = new BadgeInformation
            {
                UserId = HttpContext.GetUser()
            };

            var badgeInformation = mapper.Map<BadgeInformation>(badgeInformationRequest);

            var creationResult = await badgeInformationService.CreateAsync(badgeInformation);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{Id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<BadgeInformationResponse>
            {
                Data = mapper.Map<BadgeInformationResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }
    }
}
