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
    public class SchoolIdentityController : Controller
    {
        private readonly ISchoolIdentityService schoolIdentityService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;

        public SchoolIdentityController(ISchoolIdentityService schoolIdentityService, IMapper mapper, IUriService uriService)
        {
            this.schoolIdentityService = schoolIdentityService;

            this.mapper = mapper;     

            this.uriService = uriService;       
        }

        [HttpPost(ApiRoutes.SchoolIdentityRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] CreateSchoolIdentity createSchoolIdentityRequest)
        {
            if (createSchoolIdentityRequest == null) return BadRequest();

            string route = ApiRoutes.SchoolIdentityRoutes.Get;

            var identity = mapper.Map<SchoolIdentity>(createSchoolIdentityRequest);

            var user = mapper.Map<User>(createSchoolIdentityRequest);

            var auth = new AuthData
            {
                Password = Constants.UserContansts.RandomPassword,
                Role = Constants.UserContansts.SchoolRole
            };

            var creationResult = await schoolIdentityService.CreateAsync(identity, user, auth);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{Id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<SchoolIdentityResponse>
            {
                Data = mapper.Map<SchoolIdentityResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }
    }
}
