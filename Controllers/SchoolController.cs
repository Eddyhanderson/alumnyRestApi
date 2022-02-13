using System.Collections.Generic;
using System.Threading.Tasks;
using alumni.Contracts.V1;
using alumni.Contracts.V1.Responses;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace alumni.Controllers
{
    [ApiController]
    public class SchoolController : Controller
    {
        private readonly ISchoolService schoolService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;

        public SchoolController(ISchoolService schoolService, IMapper mapper, IUriService uriService)
        {
            this.schoolService = schoolService;

            this.mapper = mapper;     

            this.uriService = uriService;       
        }

        [HttpPost(ApiRoutes.SchoolRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] CreateSchoolRequest createSchoolRequest)
        {
            if (createSchoolRequest == null) return BadRequest();

            string route = ApiRoutes.SchoolRoutes.Get;

            var identity = mapper.Map<School>(createSchoolRequest);

            var user = mapper.Map<User>(createSchoolRequest);

            var auth = new AuthData
            {
                Password = Constants.UserContansts.RandomPassword,
                Role = Constants.UserContansts.SchoolRole
            };

            var creationResult = await schoolService.CreateAsync(identity, user, auth);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<SchoolResponse>
            {
                Data = mapper.Map<SchoolResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.SchoolRoutes.GetByUser)]    
        public async Task<IActionResult> GetByUser([FromRoute] string userId)
        {
            if (userId == null) return BadRequest();

            var school = await schoolService.GetByUserAsync(userId);

            if (school == null) return BadRequest();

            var schoolResponse = mapper.Map<SchoolResponse>(school);

            var response = new Response<SchoolResponse>(schoolResponse);

            response.Data.User.Role= Constants.UserContansts.SchoolRole;

            return Ok(response);
        }
    }
}
