using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

    public class TeacherPlaceMaterialController : ControllerBase
    {
        private readonly ITeacherPlaceMaterialService teacherPlaceMaterialService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;


        public TeacherPlaceMaterialController(ITeacherPlaceMaterialService teacherPlaceMaterialService, IMapper mapper, IUriService uriService)
        {
            this.teacherPlaceMaterialService = teacherPlaceMaterialService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [Authorize(Roles = Constants.UserContansts.SchoolRole)]
        [HttpPost(ApiRoutes.TeacherPlaceMaterialRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] TeacherPlaceMaterialRequest materialRequest)
        {
            if (materialRequest == null) return BadRequest();

            string route = ApiRoutes.TeacherPlaceMaterialRoutes.Get;

            var material = mapper.Map<TeacherPlaceMaterial>(materialRequest);

            var creationResult = await teacherPlaceMaterialService.CreateAsync(material);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{Id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<TeacherPlaceMaterialResponse>
            {
                Data = mapper.Map<TeacherPlaceMaterialResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.TeacherPlaceMaterialRoutes.GetAllByTeacherPlace)]
        public async Task<IActionResult> GetAll([FromRoute] string teacherPlaceId, [FromQuery] PaginationQuery query)
        {
            if (query == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(query);

            var searchMode = filter.SearchValue != null;

            var materials = await teacherPlaceMaterialService.GetMaterialsByTeacherPlaceAsync(teacherPlaceId, filter);

            var materialsResponse = mapper.Map<List<TeacherPlaceMaterialResponse>>(materials);

            var pageResponse = new PageResponse<TeacherPlaceMaterialResponse>(materialsResponse);

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: materialsResponse,
                                    uriService: uriService,
                                    path: ApiRoutes.TeacherPlaceMaterialRoutes.GetAllByTeacherPlace.Replace("{teacherPlaceId}", teacherPlaceId),
                                    searchMode: searchMode);

            return Ok(paginationResponse);
        }


        [HttpGet(ApiRoutes.TeacherPlaceMaterialRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            if (Id != null)
            {
                var material = await teacherPlaceMaterialService.GetTeacherPlaceMaterialAsync(Id);

                if (material == null) return BadRequest();

                var response = new Response<TeacherPlaceMaterialResponse>(mapper.Map<TeacherPlaceMaterialResponse>(material));

                return Ok(response);
            }

            return BadRequest();
        }

        [HttpGet(ApiRoutes.TeacherPlaceMaterialRoutes.GetByPost)]
        public async Task<IActionResult> GetByPost([FromRoute] string postId)
        {
            if (postId != null)
            {
                var material = await teacherPlaceMaterialService.GetTeacherPlaceMaterialByPostAsync(postId);

                if (material == null) return BadRequest();

                var response = new Response<TeacherPlaceMaterialResponse>(mapper.Map<TeacherPlaceMaterialResponse>(material));

                return Ok(response);
            }

            return BadRequest();
        }

    }
}
