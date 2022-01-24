using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alumni.Contracts.V1;
using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Requests.Queries;
using alumni.Contracts.V1.Responses;
using alumni.Data;
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
    public class TeacherPlaceStudantsController : ControllerBase
    {
        private readonly ITeacherPlaceStudantService teacherPlaceStudantService;

        private readonly IUserService userService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;

        public TeacherPlaceStudantsController(DataContext dataContext,
            ITeacherPlaceStudantService teacherPlaceStudantService,
            IUserService userService,
            IMapper mapper,
            IUriService uriService)
        {
            this.teacherPlaceStudantService = teacherPlaceStudantService;

            this.mapper = mapper;

            this.uriService = uriService;

            this.userService = userService;
        }

        [Authorize(Roles = Constants.UserContansts.StudantRole)]
        [HttpPost(ApiRoutes.TeacherPlaceStudantsRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] TeacherPlaceStudantsRequest teacherPlaceStudantsRequest)
        {
            if (teacherPlaceStudantsRequest == null) return BadRequest();

            var teacherPlaceStudants = mapper.Map<TeacherPlaceStudants>(teacherPlaceStudantsRequest);

            var studant = await userService.GetStudantAsync(HttpContext.GetUser());

            if (studant.Id != teacherPlaceStudants.StudantId) return BadRequest();

            var creationResult = await teacherPlaceStudantService.CreateAsync(teacherPlaceStudants);

            if (!creationResult.Succeded) return Conflict();

            string route = ApiRoutes.TeacherPlaceStudantsRoutes.Get;

            var parameter = new Dictionary<string, string>
                    {
                        {"{teacherPlaceId}",creationResult.Data.TeacherPlaceId },
                        {"{studantId}",creationResult.Data.StudantId }
                    };

            var creationResponse = new CreationResponse<TeacherPlaceStudantsResponse>
            {
                Data = mapper.Map<TeacherPlaceStudantsResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }


        [HttpGet(ApiRoutes.TeacherPlaceStudantsRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string teacherPlaceId, string studantId)
        {
            if (teacherPlaceId == null || studantId == null) return BadRequest();

            var result = await teacherPlaceStudantService.GetAsync(teacherPlaceId, studantId);

            var response = mapper.Map<TeacherPlaceStudantsResponse>(result);

            return Ok(new Response<TeacherPlaceStudantsResponse>(response));
        }

        [AllowAnonymous]
        [HttpPut(ApiRoutes.TeacherPlaceStudantsRoutes.Update)]
        public async Task<IActionResult> Update([FromRoute] string teacherPlaceId, [FromRoute] string studantId,
            [FromBody] TeacherPlaceStudantsRequest teacherPlaceStudantsRequest)
        {
            if (teacherPlaceId == null || studantId == null) return BadRequest();

            var teacherPlaceStudant = mapper.Map<TeacherPlaceStudants>(teacherPlaceStudantsRequest);

            var succeded = await teacherPlaceStudantService.UpdateAsync(teacherPlaceId, studantId, teacherPlaceStudant);

            if (succeded)
                return NoContent();

            var exists = await teacherPlaceStudantService.ObjectExists(studantId, teacherPlaceId);

            if (!exists) return NotFound();

            return BadRequest();
        }

    }
}
