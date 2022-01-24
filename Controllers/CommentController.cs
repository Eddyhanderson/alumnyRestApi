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
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;


        public CommentController(ICommentService commentService, IMapper mapper, IUriService uriService)
        {
            this.commentService = commentService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.CommentRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] CommentRequest commentRequest)
        {
            if (commentRequest == null) return BadRequest();

            string route = ApiRoutes.CommentRoutes.Get;

            var comment = mapper.Map<Comment>(commentRequest);

            var creationResult = await commentService.CreateAsync(comment);

            if (!creationResult.Succeded) return Conflict();

            var parameter = new Dictionary<string, string>
                    {
                        {"{Id}",creationResult.Data.Id }
                    };

            var creationResponse = new CreationResponse<CommentResponse>
            {
                Data = mapper.Map<CommentResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.CommentRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] CommentQuery commentQuery, [FromQuery] PaginationQuery query)
        {
            if (query == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(query);
             
            var searchMode = filter.SearchValue != null;

            var commentsResult = await commentService.GetCommentsAsync(commentQuery, filter);

            var pageResponse = new PageResponse<CommentResponse>
            {
                Data = mapper.Map<IEnumerable<Comment>, IEnumerable<CommentResponse>>(commentsResult.Data),
                TotalElements = commentsResult.TotalElements
            };

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);           

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: pageResponse.Data,
                                    uriService: uriService,
                                    path: ApiRoutes.CommentRoutes.GetAll,
                                    searchMode: searchMode);

            paginationResponse.TotalElements = commentsResult.TotalElements;

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.CommentRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            if (Id != null)
            {
                var comment = await commentService.GetCommentAsync(Id);

                if (comment == null) return NotFound();

                var response = new Response<CommentableResponse>(mapper.Map<CommentableResponse>(comment));

                return Ok(response);
            }

            return BadRequest();
        }
    }
}
