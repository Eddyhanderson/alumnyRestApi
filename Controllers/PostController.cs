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
    public class PostController : ControllerBase
    {
        private readonly IPostService postService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;


        public PostController(IPostService postService, IMapper mapper, IUriService uriService)
        {
            this.postService = postService;

            this.mapper = mapper;

            this.uriService = uriService;
        }
        

        [HttpGet(ApiRoutes.PostRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
        {
            if (query == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(query);

            var searchMode = filter.SearchValue != null;

            var posts = await postService.GetPostsAsync(filter);

            var postsResponse = mapper.Map<List<PostResponse>>(posts);

            var pageResponse = new PageResponse<PostResponse>(postsResponse);

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: postsResponse,
                                    uriService: uriService,
                                    path: ApiRoutes.PostRoutes.GetAll,
                                    searchMode: searchMode);

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.PostRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            if (Id != null)
            {
                var post = await postService.GetPostAsync(Id);

                if (post == null) return BadRequest();

                var response = new Response<PostResponse>(mapper.Map<PostResponse>(post));

                return Ok(response);
            }

            return BadRequest();
        }
    }
}
