using alumni.Contracts.V1.Requests.Queries;
using alumni.Data;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Services
{
    public class CommentService : ICommentService
    {
        private readonly DataContext dataContext;

        private readonly IPostService postService;

        public CommentService(DataContext dataContext, IPostService postService)
        {
            this.dataContext = dataContext;

            this.postService = postService;
        }

        public async Task<CreationResult<Comment>> CreateAsync(Comment comment)
        {
            if (comment == null)
                return FailCreation();

                var postStt = await postService.CreateAsync(PostsTypes.Comment);

                if (!postStt.Succeded) return FailCreation();

                comment.PostId = postStt.Data.Id;
            

            var newComment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                PostId = comment.PostId,
                Content = comment.Content,
                ComentableId = comment.ComentableId
            };

            try
            {
                await dataContext.Comments.AddAsync(newComment);

                var stt = await dataContext.SaveChangesAsync();

                return new CreationResult<Comment>
                {
                    Data = newComment,
                    Succeded = true
                };

            }
            catch (DbException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<Video> GetVideoByLessonIdAsync(string lessonId)
        {
            if (lessonId == null) return null;

            // TODO: To revise implementation
            var video = await dataContext.Videos.SingleOrDefaultAsync(/*v => v.LessonId == lessonId*/);

            return video;
        }

        public async Task<Comment> GetCommentAsync(string id)
        {
            if (id == null) return null;

            var normalState = Constants.SituationsObjects.NormalSituation;

            var comment = await dataContext
                .Comments.Include(c => c.Post).ThenInclude(p => p.User)
                .SingleOrDefaultAsync(c => c.Id == id && c.Post.Situation == normalState);

            return comment;
        }

        public Task<bool> ObjectExists(string id)
        {
            return dataContext.Comments.AnyAsync(c => c.Id == id);
        } 

        /*public async Task<PageResult<Comment>> GetCommentsAsync(CommentQuery query, PaginationFilter filter = null)
        {
            var normalState = Constants.SituationsObjects.NormalSituation;

            if (query == null) return null;

            var comments = dataContext.Comments
                .Include(c => c.Post).ThenInclude(p => p.User).AsQueryable();

            if (query.CommentableId != null)
            {
                comments = comments.Where(c => c.ComentableId == query.CommentableId && c.Post.Situation == normalState)
                .AsQueryable();
            }                            

            return await GetPaginationAsync(comments, filter);
        }*/

        private async Task<PageResult<Comment>> GetPaginationAsync(IQueryable<Comment> comments, PaginationFilter filter)
        {
            var searchMode = filter.SearchValue != null;
            var totalElement = await comments.CountAsync();

            if (searchMode)
            {
                var sv = filter.SearchValue;

                comments = comments
                    .Where(c => c.Content.Contains(sv));
            }

            if (filter.PageNumber >= 0 || filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                comments = comments.Skip(skip).Take(filter.PageSize);
            }

            var data = await comments.OrderBy(c => c.Post.CreateAt).ToListAsync();

            var page = new PageResult<Comment>
            {
                Data = data,
                TotalElements = totalElement
            };

            return page;
        }

        private CreationResult<Comment> FailCreation()
        {
            return new CreationResult<Comment>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }

    }
}
