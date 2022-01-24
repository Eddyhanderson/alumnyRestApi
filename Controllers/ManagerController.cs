using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alumni.Contracts.V1;
using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Responses;
using alumni.Domain;
using alumni.IServices;
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
        public async Task<IActionResult> Create([FromBody] ManagerRequest managerRequest)
        {
            if (managerRequest == null) return BadRequest();

            string route = ApiRoutes.ManagerRoutes.Get;

            var manager = mapper.Map<Manager>(managerRequest);

            var creationResult = await managerService.CreateAsync(manager);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{Id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<ManagerResponse>
            {
                Data = mapper.Map<ManagerResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
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
