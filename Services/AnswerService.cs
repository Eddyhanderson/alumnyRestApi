using alumni.Contracts.V1.Requests.Queries;
using alumni.Data;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Services
{
    public class AnswerService: IAnswerService
    {
        private readonly DataContext dataContext;
        private readonly IPostService postService;

        public AnswerService(DataContext dataContext, IPostService postService)
        {
            this.dataContext = dataContext;
            this.postService = postService;
        }

        public async Task<CreationResult<Answer>> CreateAsync(Answer answer)
        {
            if (answer == null) return null;

            var postStt = await postService.CreateAsync(PostsTypes.QuestionAnswer);

            if (!postStt.Succeded) FailCreation();

            answer.PostId = postStt.Data.Id;

            try
            {
                var newAnswer = new Answer
                {
                    Id = Guid.NewGuid().ToString(),
                    Content = answer.Content,
                    QuestionId = answer.QuestionId,
                    PostId = answer.PostId
                };

                await dataContext.Answers.AddAsync(newAnswer);

                await dataContext.SaveChangesAsync();

                return new CreationResult<Answer>
                {
                    Data = newAnswer,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<PageResult<Answer>> GetAllAsync(PaginationFilter filter, AnswerQuery query)
        {
            var normalState = Constants.SituationsObjects.NormalSituation;
            var answer = dataContext.Answers.Include(a => a.Post).ThenInclude(p => p.User).AsQueryable();
            
            if (query.QuestionId != null)
            {
                answer = answer.Where(a => a.QuestionId == query.QuestionId && a.Post.Situation == normalState);               

                return await GetPaginationAsync(answer, filter);

            }

            return null;
        }

        public async Task<Answer> GetAsync(string id)
        {
            if (id == null) return null;

            return await dataContext.Answers.Include(a => a.Post).ThenInclude(p => p.User).SingleOrDefaultAsync(a => a.Id == id);
        }

        private CreationResult<Answer> FailCreation()
        {
            return new CreationResult<Answer>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private async Task<PageResult<Answer>> GetPaginationAsync(IQueryable<Answer> answer, PaginationFilter filter)
        {
            var searchMode = filter.SearchValue != null;
            var totalElement = await answer.CountAsync();

            if (searchMode)
            {
                var sv = filter.SearchValue;

                answer = answer
                    .Where(q => q.Content.Contains(filter.SearchValue));
            }

            if (filter.PageNumber >= 0 || filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                answer = answer.Skip(skip).Take(filter.PageSize);
            }

            var data = await answer.OrderByDescending(l => l.Post.CreateAt).ToListAsync();

            var page = new PageResult<Answer>
            {
                Data = data,
                TotalElements = totalElement
            };

            return page;
        }
    }
}
