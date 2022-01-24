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
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService teacherService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;


        public TeacherController(ITeacherService teacherService, IMapper mapper, IUriService uriService)
        {
            this.teacherService = teacherService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [Authorize(Roles = Constants.UserContansts.TeacherRole)]
        [HttpPost(ApiRoutes.TeacherRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] TeacherRequest teacherRequest)
        {
            if (teacherRequest == null) return BadRequest();

            string route = ApiRoutes.TeacherRoutes.Get;

            var teacher = mapper.Map<Teacher>(teacherRequest);

            var creationResult = await teacherService.CreateAsync(teacher);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{Id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<TeacherResponse>
            {
                Data = mapper.Map<TeacherResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.TeacherRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
        {
            if (query == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(query);

            filter.UserId = HttpContext.GetUser();

            var searchMode = filter.SearchValue != null;

            var pageResult = await teacherService.GetTeachersAsync(filter);            

            var pageResponse = new PageResponse<TeacherResponse>
            {
                Data = mapper.Map<IEnumerable<TeacherResponse>>(pageResult.Data),
                TotalElements = pageResult.TotalElements
            };

            foreach(var t in pageResponse.Data)
            {
                t.TeacherPlaceQnt = await teacherService.TeacherPlaceCountAsync(t.Id);
            }            

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: pageResponse.Data,
                                    uriService: uriService,
                                    path: ApiRoutes.DisciplineRoutes.GetAll,
                                    searchMode: searchMode);

            paginationResponse.TotalElements = pageResult.TotalElements;

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.TeacherRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            if (Id != null)
            {
                var teacher = await teacherService.GetTeacherAsync(Id);

                if (teacher == null) return BadRequest();

                var response = new Response<TeacherResponse>(mapper.Map<TeacherResponse>(teacher));

                return Ok(response);
            }

            return BadRequest();
        }

   

    }
}
