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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace alumni.Controllers
{
    [ApiController]
    public class ManagerController : Controller
    {
        private readonly IManagerService managerService;
        private readonly IUriService uriService;
        private readonly IMapper mapper;

        public ManagerController(IManagerService managerService, IUriService uriService, IMapper mapper)
        {
            this.managerService = managerService;
            this.uriService = uriService;
            this.mapper = mapper;
        }

        [HttpPost(ApiRoutes.ManagerRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] CreateManagerRequest managerRequest)
        {
            if (managerRequest == null) return BadRequest();

            var manager = mapper.Map<Manager>(managerRequest);

            var user = mapper.Map<User>(managerRequest);

            var auth = new AuthData
            {
                Password = managerRequest.Password,
                Role = Constants.UserContansts.AdminRole
            };

            var authResult = await managerService.CreateAsync(manager, user, auth);

            var authResponse = mapper.Map<AuthResultResponse>(authResult);

            if (!authResponse.Authenticated) return Unauthorized(authResponse);            

            return Ok(authResponse);
        }

        [HttpGet(ApiRoutes.ManagerRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            if (id == null) return BadRequest();

            var manager = await managerService.GetAsync(id);

            if (manager == null) return BadRequest();

            var managerResponse = mapper.Map<ManagerResponse>(manager);

            return Ok(new Response<ManagerResponse>(managerResponse));
        }

        [HttpGet(ApiRoutes.ManagerRoutes.GetByUser)]
        public async Task<IActionResult> GetByUser([FromRoute] string userId)
        {
            if (userId == null) return BadRequest();

            var manager = await managerService.GetByUserAsync(userId);

            if (manager == null) return BadRequest();

            var managerResponse = mapper.Map<ManagerResponse>(manager);

            return Ok(new Response<ManagerResponse>(managerResponse));
        }
    }
}
