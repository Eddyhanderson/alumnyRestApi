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
using Microsoft.AspNetCore.Mvc;

namespace alumni.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ArticleController : Controller
    {
        private readonly IMapper mapper;

        private readonly IArticleService articleService;

        private readonly IUriService uriService;

        public ArticleController(IMapper mapper, IArticleService articleService, IUriService uriService)
        {
            this.mapper = mapper;

            this.articleService = articleService;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.ArticleRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] ArticleRequest articleRequest)
        {
            if (articleRequest == null) return BadRequest();

            var article = mapper.Map<Article>(articleRequest);

            var createResult = await articleService.CreateAsync(article);

            if (!createResult.Succeded) Conflict(createResult.Messages);

            Dictionary<string, string> parameter = new Dictionary<string, string>
            {
                { "{Id}", createResult.Data.Id },
            };

            var creationResponse = new CreationResponse<ArticleResponse>
            {
                Data = mapper.Map<ArticleResponse>(createResult.Data),
                Messages = createResult.Messages,
                Succeded = true,
                GetUri = uriService.GetModelUri(parameter, ApiRoutes.ArticleRoutes.Get)
            };

            return Created(creationResponse.GetUri, creationResponse);

        }

        [HttpGet(ApiRoutes.ArticleRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string Id)
        {
            if (Id == null) return BadRequest();

            var article = await articleService.GetArticleAsync(Id);

            if (article == null) return BadRequest();

            var articleResponse = mapper.Map<ArticleResponse>(article);

            return Ok(new Response<ArticleResponse>(articleResponse));
        }

        [HttpGet(ApiRoutes.ArticleRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query, [FromQuery] ArticleQuery articleQuery)
        {
            if (query == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(query);

            var searchMode = filter.SearchValue != null;

            var pageResult = await articleService.GetArticlesAsync(filter, articleQuery);

            var pageResponse = new PageResponse<ArticleResponse>
            {
                Data = mapper.Map<IEnumerable<Article>, IEnumerable<ArticleResponse>>(pageResult.Data),
                TotalElements = pageResult.TotalElements
            };

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: pageResponse.Data,
                                    uriService: uriService,
                                    path: ApiRoutes.ArticleRoutes.GetAll,
                                    searchMode: searchMode);

            paginationResponse.TotalElements = pageResult.TotalElements;

            return Ok(paginationResponse);
        }

        [HttpPut(ApiRoutes.ArticleRoutes.Update)]
        public async Task<IActionResult> Update([FromRoute] string articleId, [FromBody] ArticleRequest articleRequest)
        {
            if (articleId == null || articleRequest == null) return BadRequest();

            var article = mapper.Map<Article>(articleRequest);

            var dataResult = await articleService.UpdateAsync(articleId, article);

            if (dataResult != null)
            {
                return Ok(new Response<ArticleResponse>(mapper.Map<ArticleResponse>(dataResult)));
            }

            return Conflict();
        }
    }
}
