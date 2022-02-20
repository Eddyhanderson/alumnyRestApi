using alumni.Contracts.V1.Requests.Queries;
using alumni.Data;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Services
{
    public class ArticleService : IArticleService
    {
        private readonly DataContext dataContext;

        public ArticleService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<CreationResult<Article>> CreateAsync(Article article)
        {
            if (article == null)
                return FailCreation();

            var newArticle = new Article
            {
                Id = Guid.NewGuid().ToString(),
                Name = article.Name,
                Delta = article.Delta,
                Draft = true,
                ModuleId = article.ModuleId,
                LastChange = DateTime.UtcNow,
                Situation = Constants.SituationsObjects.NormalSituation
            };

            try
            {
                await dataContext.Articles.AddAsync(newArticle);

                var stt = await dataContext.SaveChangesAsync();

                return new CreationResult<Article>
                {
                    Data = newArticle,
                    Succeded = true
                };

            }
            catch (DbException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<Article> GetArticleAsync(string id)
        {
            if (id == null) return null;

            return await dataContext.Articles.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PageResult<Article>> GetArticlesAsync(PaginationFilter filter, ArticleQuery articleQuery)
        {
            if (filter == null) return null;

            var articles = dataContext.Articles.Include(a => a.Module)
            .ThenInclude(m => m.Formation).ThenInclude(f => f.School)
            .Where(a => a.Situation == Constants.SituationsObjects.NormalSituation);

            if (articleQuery != null)
            {
                if(articleQuery.SchoolId != null)
                    articles = articles.Where(a => a.Module.Formation.SchoolId == articleQuery.SchoolId);
                
                if(articleQuery.FormationId != null)
                    articles = articles.Where(a => a.Module.FormationId == articleQuery.FormationId);

                if (articleQuery.ModuleId != null)
                    articles = articles.Where(a => a.ModuleId == articleQuery.ModuleId);

                if (articleQuery.Draft)
                    articles = articles.Where(a => a.Draft == true);

            }

            return await GetPaginationAsync(articles, filter);
        }

        private async Task<PageResult<Article>> GetPaginationAsync(IQueryable<Article> articles, PaginationFilter filter)
        {
            var searchMode = filter.SearchValue != null;
            var totalElement = await articles.CountAsync();

            if (searchMode)
            {
                var sv = filter.SearchValue;

                articles = articles
                    .Where(a => a.Delta.Contains(filter.SearchValue));
            }

            if (filter.PageNumber >= 0 || filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                articles = articles.Skip(skip).Take(filter.PageSize);
            }

            var data = await articles.OrderByDescending(a => a.LastChange).ToListAsync();

            var page = new PageResult<Article>
            {
                Data = data,
                TotalElements = totalElement
            };

            return page;
        }

        public async Task<Article> UpdateAsync(string articleId, Article article)
        {
            if (articleId != article.Id) return null;

            var exists = await dataContext.Articles.AnyAsync(a => a.Id == articleId);

            try
            {
                if (exists)
                {
                    article.LastChange = DateTime.UtcNow;
                    dataContext.Entry(article).State = EntityState.Modified;
                    await dataContext.SaveChangesAsync();

                    return article;
                }
                else
                {
                    var newArticle = new Article
                    {
                        Id = Guid.NewGuid().ToString(),
                        Delta = article.Delta,
                        Draft = true,
                        ModuleId = article.ModuleId,
                        LastChange = DateTime.UtcNow,
                        Situation = Constants.SituationsObjects.NormalSituation
                    };

                    await dataContext.Articles.AddAsync(newArticle);
                    await dataContext.SaveChangesAsync();

                    return newArticle;
                }
            }
            catch (DbException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private CreationResult<Article> FailCreation()
        {
            return new CreationResult<Article>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }
    }
}
