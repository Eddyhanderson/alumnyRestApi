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
    public class TopicService : ITopicService
    {
        private readonly DataContext dataContext;

        private readonly IPostService postService;

        public TopicService(DataContext dataContext, IPostService postService)
        {
            this.dataContext = dataContext;

            this.postService = postService;
        }
        
        private CreationResult<Topic> FailCreation()
        {
            return new CreationResult<Topic>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        public async Task<CreationResult<Topic>> CreateAsync(Topic topic)
        {
            if (topic == null)
                return FailCreation();

            if (topic.PostId == null)
            {
                var postStt = await postService.CreateAsync(PostsTypes.Topic);

                if (!postStt.Succeded) return FailCreation();

                topic.PostId = postStt.Data.Id;
            }

            try
            {
                var newTopic = new Topic
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = topic.Description,
                    TeacherPlaceId = topic.TeacherPlaceId,
                    DisciplineTopicId = topic.DisciplineTopicId,
                    PhotoProfile = topic.PhotoProfile,
                    PostId = topic.PostId
                };

                await dataContext.Topics.AddAsync(newTopic);

                var stt = await dataContext.SaveChangesAsync();

                if (stt > 0)
                    return new CreationResult<Topic>
                    {
                        Data = newTopic,
                        Succeded = true
                    };
                else return FailCreation();
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }  
        }

        public async Task<Topic> GetTopicAsync(string topicId)
        {
            if (topicId == null) return null;

            return await dataContext.Topics
                .Include(t => t.TeacherPlace).ThenInclude(tp => tp.Discipline)
                .Include(t => t.DisciplineTopic)
                .Include(t => t.Post)
                .SingleOrDefaultAsync(t => t.Id == topicId
                && t.Post.Situation == Constants.SituationsObjects.NormalSituation); ;
        }

        public async Task<IEnumerable<Topic>> GetTopicsAsync(PaginationFilter filter = null, TopicQuery query = null)
        {
            var topics = dataContext.Topics
                .Include(t => t.TeacherPlace).ThenInclude(tp => tp.Discipline)
                .Include(t => t.DisciplineTopic)
                .Include(t => t.Post)
                .Where(t => t.Post.Situation == Constants.SituationsObjects.NormalSituation)
                .AsQueryable();

            if (query != null && query.SchoolId != null)
                topics = (from t in topics
                          from tp in dataContext.TeacherPlaces
                          from s in dataContext.Schools
                          where t.TeacherPlaceId == tp.Id && t.TeacherPlaceId == query.TeacherPlaceId &&
                          tp.TeacherId == query.TeacherId && tp.SchoolId == query.SchoolId
                          select t);

            if (query != null && query.TeacherId != null)
                topics = (from t in topics
                          from tp in dataContext.TeacherPlaces
                          where t.TeacherPlaceId == tp.Id && tp.TeacherId == query.TeacherId
                          select t);

            if (query != null && query.TeacherPlaceId != null)
                topics = (from t in topics
                          where t.TeacherPlaceId == query.TeacherPlaceId
                          select t);

            if (filter == null) return await topics.ToListAsync();

            return await GetPaginationAsync(topics, filter);
        }     

        public async Task<int> LessonCountAsync(string topicId)
        {
            return await dataContext.Lessons.Where(l => l.TopicId == topicId).CountAsync();
        }

        public async Task<int> OpenLessonCountAsync(string topicId)
        {
            return await dataContext.Lessons.Where(l => l.TopicId == topicId && l.Public).CountAsync();
        }

        public async Task<int> QuestionCountAsync(string topicId)
        {
            return await (from l in dataContext.Lessons
                          from q in dataContext.Questions
                          where l.TopicId == topicId && l.Id == q.LessonId
                          select l).CountAsync();
        }

        public async Task<int> SolvedQuestionCountAsync(string topicId)
        {
            return await (from l in dataContext.Lessons
                          from q in dataContext.Questions
                          where l.TopicId == topicId && l.Id == q.LessonId &&
                          q.Situation == QuestionSituations.Solved.ToString("g")
                          select l).CountAsync();
        }

        public async Task<int> AnswerCountAsync(string topicId)
        {
            return await (from l in dataContext.Lessons
                          from a in dataContext.Answers
                          from q in dataContext.Questions
                          where l.TopicId == topicId && l.Id == q.LessonId && a.QuestionId == q.Id
                          select a).CountAsync();
        }

        public async Task<int> CommentCountAsync(string topicId)
        {
            return await (from t in dataContext.Topics
                          from c in dataContext.Comments
                          where t.Post.CommentableId == c.ComentableId && t.Id == topicId
                          select c).CountAsync();
        }

        public async Task<int> StudantAnswerCount(string topicId)
        {
            return await (from s in dataContext.Studants
                          from a in dataContext.Answers
                          from l in dataContext.Lessons
                          from q in dataContext.Questions
                          from tp in dataContext.TeacherPlaces
                          from t in dataContext.Topics
                          where a.QuestionId == q.Id && q.LessonId == l.Id && l.TeacherPlaceId == tp.Id
                          && a.Post.UserId == s.UserId && tp.Id == t.TeacherPlaceId && t.Id == topicId
                          select a).CountAsync();
        }

        public async Task<int> TeacherAnswerCount(string topicId)
        {
            return await (from q in dataContext.Questions
                          from l in dataContext.Lessons
                          from a in dataContext.Answers
                          from tp in dataContext.TeacherPlaces
                          from t in dataContext.Teachers
                          from tt in dataContext.Topics
                          where q.LessonId == l.Id && l.TeacherPlaceId == tp.Id && a.QuestionId == q.Id
                          && a.Post.UserId == t.UserId && t.Id == tp.TeacherId && tp.Id == tt.TeacherPlaceId && tt.Id == topicId
                          select a).CountAsync();
        }

        private async Task<IEnumerable<Topic>> GetPaginationAsync(IQueryable<Topic> topics, PaginationFilter filter)
        {
            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                topics = topics
                    .Where(t => t.DisciplineTopic.Name.Contains(sv));
            }

            if (filter.PageNumber >= 0 && filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                topics = topics.Skip(skip).Take(filter.PageSize);
            }

            return await topics.ToListAsync();
        }

        public Task<Topic> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Topic> UpdateAsync(Topic entity, string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Topic entity)
        {
            throw new NotImplementedException();
        }       
    }
}
