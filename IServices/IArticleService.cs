using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IArticleService
    {
        Task<CreationResult<Article>> CreateAsync(Article article);

        Task<Article> GetArticleAsync(string id);

        Task<PageResult<Article>> GetArticlesAsync(PaginationFilter filter, ArticleQuery articleQuery);

        Task<Article> UpdateAsync(string articleId, Article article);
    }
}
