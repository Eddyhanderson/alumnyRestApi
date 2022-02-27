using alumni.Data;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext dataContext;

        private readonly ICommentableService commentableService;

        private readonly IUserService userService;

        private readonly IServiceProvider serviceProvider;

        public PostService(DataContext dataContext,
            ICommentableService commentableService,
            IServiceProvider serviceProvider,
            IUserService userService)
        {
            this.dataContext = dataContext;

            this.commentableService = commentableService;

            this.serviceProvider = serviceProvider;

            this.userService = userService;


        }

        public async Task<CreationResult<Post>> CreateAsync(PostsTypes postsTypes)
        {
            Post post = new Post();

            var stt = await commentableService.CreateAsync();

            if (!stt.Succeded) return FailCreation();

            post.CommentableId = stt.Data.Id;

            var accessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

            post.UserId = accessor.HttpContext.GetUser();

            var newPost = new Post
            {
                Id = Guid.NewGuid().ToString(),
                CommentableId = post.CommentableId,
                CreateAt = DateTime.UtcNow,
                PostType = postsTypes.ToString("g"),
                Situation = Constants.SituationsObjects.NormalSituation,
                UserId = post.UserId
            };

            try
            {
                await dataContext.Posts.AddAsync(newPost);

                await dataContext.SaveChangesAsync();

                return new CreationResult<Post>
                {
                    Data = newPost,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<IEnumerable<Post>> GetPostsAsync(PaginationFilter filter = null)
        {
            var user = await GetUserAsync();

            if (user == null) return null;

            var studant = await dataContext.Studants
                .SingleOrDefaultAsync(s => s.UserId == user.Id);

            if (studant == null) return null;

            var normalState = Constants.SituationsObjects.NormalSituation;

            List<Post> posts = new List<Post>();

        

            if (filter == null)
                return posts.OrderByDescending(p => p.CreateAt).ToList();

            return GetPaginationAsync(posts.AsQueryable(), filter);
        }

        public async Task<Post> GetPostAsync(string id)
        {
            if (id == null) return null;

            return await dataContext.Posts.Include(p => p.User).SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ObjectExists(string id)
        {
            return await dataContext.Posts.AnyAsync(p => p.Id == id);
        }

        private async Task<User> GetUserAsync()
        {
            var accessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

            var user = await userService.GetUserAsync(accessor.HttpContext.GetUser());

            return user;
        }

        private CreationResult<Post> FailCreation()
        {
            return new CreationResult<Post>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private IEnumerable<Post> GetPaginationAsync(IQueryable<Post> posts, PaginationFilter filter)
        {
            if (filter.PageNumber >= 0 || filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                posts = posts.Skip(skip).Take(filter.PageSize);
            }

            return posts.OrderByDescending(p => p.CreateAt);
        }
    }
}
