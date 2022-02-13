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
    public class TeacherPlaceMessageController : ControllerBase
    {

        private readonly ITeacherPlaceMessageService teacherPlaceMessageService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;

        public TeacherPlaceMessageController(ITeacherPlaceMessageService teacherPlaceMessageService, IMapper mapper, IUriService uriService)
        {
            this.teacherPlaceMessageService = teacherPlaceMessageService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [Authorize(Roles = Constants.UserContansts.SchoolRole)]
        [HttpPost(ApiRoutes.TeacherPlaceMessageRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] TeacherPlaceMessageRequest messageRequest)
        {
            if (messageRequest == null) return BadRequest();

            string route = ApiRoutes.TeacherPlaceMessageRoutes.Get;

            var message = mapper.Map<TeacherPlaceMessage>(messageRequest);

            var creationResult = await teacherPlaceMessageService.CreateAsync(message);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{Id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<TeacherPlaceMessageResponse>
            {
                Data = mapper.Map<TeacherPlaceMessageResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.TeacherPlaceMessageRoutes.GetAllByTeacherPlace)]
        public async Task<IActionResult> GetAll([FromRoute] string teacherPlaceId, [FromQuery] PaginationQuery query)
        {
            if (query == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(query);

            var searchMode = filter.SearchValue != null;

            var messages = await teacherPlaceMessageService.GetMessagesByTeacherPlaceAsync(teacherPlaceId, filter);

            var messagesResponse = mapper.Map<List<TeacherPlaceMessageResponse>>(messages);

            var pageResponse = new PageResponse<TeacherPlaceMessageResponse>(messagesResponse);

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: messagesResponse,
                                    uriService: uriService,
                                    path: ApiRoutes.TeacherPlaceMessageRoutes.GetAllByTeacherPlace.Replace("{teacherPlaceId}", teacherPlaceId),
                                    searchMode: searchMode);

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.TeacherPlaceMessageRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            if (Id != null)
            {
                var message = await teacherPlaceMessageService.GetTeacherPlaceMessageAsync(Id);

                if (message == null) return BadRequest();

                var response = new Response<TeacherPlaceMessageResponse>(mapper.Map<TeacherPlaceMessageResponse>(message));

                return Ok(response);
            }

            return BadRequest();
        }

        [HttpGet(ApiRoutes.TeacherPlaceMessageRoutes.GetByPost)]
        public async Task<IActionResult> GetByPost([FromRoute] string postId)
        {
            if (postId != null)
            {
                var message = await teacherPlaceMessageService.GetTeacherPlaceMessageByPostAsync(postId);

                if (message == null) return BadRequest();

                var response = new Response<TeacherPlaceMessageResponse>(mapper.Map<TeacherPlaceMessageResponse>(message));

                return Ok(response);
            }

            return BadRequest();
        }
    }
}
