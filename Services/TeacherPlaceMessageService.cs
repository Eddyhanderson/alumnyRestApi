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
    public class TeacherPlaceMessageService : ITeacherPlaceMessageService
    {
        private readonly DataContext dataContext;

        private readonly IPostService postService;

        private readonly IServiceProvider serviceProvider;

        public TeacherPlaceMessageService(DataContext dataContext,
            IPostService postService,
            IServiceProvider serviceProvider)
        {
            this.dataContext = dataContext;

            this.postService = postService;

            this.serviceProvider = serviceProvider;
        }

        public async Task<CreationResult<TeacherPlaceMessage>> CreateAsync(TeacherPlaceMessage message)
        {
            if (message == null) return null;

            var postStt = await postService.CreateAsync(PostsTypes.TeacherMessage);

            if (!postStt.Succeded) FailCreation();

            message.PostId = postStt.Data.Id;

            var teacher = await dataContext.Teachers.SingleOrDefaultAsync(t => t.UserId == GetUserIdRequest());

            if (teacher == null) return null;

            if (message.TeacherPlaceId == null)
                return null;

            var isOwner = (await dataContext
                    .TeacherPlaces.SingleOrDefaultAsync(tp => tp.Id == message.TeacherPlaceId && tp.TeacherId == teacher.Id)) != null;

            if (!isOwner) return null;

            try
            {
                var newMessage = new TeacherPlaceMessage
                {
                    Id = Guid.NewGuid().ToString(),
                    Message = message.Message,
                    PostId = message.PostId,
                    TeacherPlaceId = message.TeacherPlaceId,
                };

                await dataContext.TeacherPlaceMessages.AddAsync(newMessage);

                await dataContext.SaveChangesAsync();

                return new CreationResult<TeacherPlaceMessage>
                {
                    Data = newMessage,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<IEnumerable<TeacherPlaceMessage>> GetMessagesByTeacherPlaceAsync(string teacherPlaceId, PaginationFilter filter = null)
        {
            var messages = dataContext
                .TeacherPlaceMessages
                .Include(tpm => tpm.Post)
                .Where(tpm => tpm.TeacherPlaceId == teacherPlaceId && tpm.Post.Situation == Constants.SituationsObjects.NormalSituation);

            return await GetPaginationAsync(messages, filter);
        }

        public async Task<TeacherPlaceMessage> GetTeacherPlaceMessageAsync(string id)
        {
            if (id == null) return null;

            var message = await dataContext.TeacherPlaceMessages
                .Include(tpm => tpm.Post)
                .SingleOrDefaultAsync(m => m.Id == id && m.Post.Situation == Constants.SituationsObjects.NormalSituation);

            return message;
        }

        public async Task<TeacherPlaceMessage> GetTeacherPlaceMessageByPostAsync(string postId)
        {
            if (postId == null) return null;

            var message = await (from tpm in dataContext.TeacherPlaceMessages
                                 from p in dataContext.Posts
                                 where tpm.PostId == postId && p.Id == postId && p.Situation == Constants.SituationsObjects.NormalSituation
                                 select tpm).SingleOrDefaultAsync();

            return message;
        }

        public Task<bool> ObjectExists(string id)
        {
            throw new NotImplementedException();
        }

        private async Task<IEnumerable<TeacherPlaceMessage>> GetPaginationAsync(IQueryable<TeacherPlaceMessage> messages, PaginationFilter filter)
        {
            if (filter == null)
                return await messages.ToListAsync();

            if (messages == null) return null;

            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                messages = messages
                    .Where(m => m.Message.Contains(sv));

            }

            if (filter.PageNumber >= 0 || filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                messages = messages.Skip(skip).Take(filter.PageSize);
            }

            return await messages.ToListAsync();
        }

        private CreationResult<TeacherPlaceMessage> FailCreation()
        {
            return new CreationResult<TeacherPlaceMessage>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private string GetUserIdRequest()
        {
            var accessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

            var user = accessor.HttpContext.GetUser();

            return user;
        }


    }
}
