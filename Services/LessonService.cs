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
    public class LessonService : ILessonService
    {
        private readonly DataContext dataContext;

        private readonly IPostService postService;

        private readonly IArticleService articleService;


        public LessonService(
            DataContext dataContext,
            IPostService postService,
            IArticleService articleService)
        {
            this.dataContext = dataContext;

            this.postService = postService;

            this.articleService = articleService;

        }

        public async Task<CreationResult<Lesson>> CreateAsync(Lesson lesson)
        {
            if (lesson == null) return null;

            try
            {
                CreationResult<Post> postStt = new CreationResult<Post>();

                if (lesson.PostId == null)
                {
                    postStt = await postService.CreateAsync(PostsTypes.Lesson);

                    if (!postStt.Succeded) return FailCreation();

                    lesson.PostId = postStt.Data.Id;
                }

                if (lesson.TopicId == null)
                    return FailCreation();

                if (lesson.ArticleId == null && lesson.VideoId == null)
                    return FailCreation();

                if (lesson.LessonType != LessonTypes.Video.ToString("g") && lesson.LessonType != LessonTypes.Article.ToString("g"))
                {
                    return FailCreation();
                }

                if (lesson.LessonType == LessonTypes.Article.ToString("g"))
                {
                    var article = dataContext.Articles.FirstOrDefault(l => l.Id == lesson.ArticleId);

                    if (article == null) return null;

                    article.Draft = false;
                    

                    var artStt = await articleService.UpdateAsync(lesson.ArticleId, article);

                    if (artStt == null) return null;
                }

                var newLesson = new Lesson
                {
                    Id = Guid.NewGuid().ToString(),
                    PostId = lesson.PostId,
                    BackgroundPhotoPath = lesson.BackgroundPhotoPath,
                    Description = lesson.Description,
                    TopicId = lesson.TopicId,
                    Sequence = await BuildLessonSequence(lesson.TopicId),
                    Views = 0,
                    Public = lesson.Public,
                    TeacherPlaceId = lesson.TeacherPlaceId,
                    Title = lesson.Title,
                    ArticleId = lesson.ArticleId,
                    VideoId = lesson.VideoId,
                    LessonType = lesson.LessonType
                };

                await dataContext.Lessons.AddAsync(newLesson);
                await dataContext.SaveChangesAsync();


                return new CreationResult<Lesson>
                {
                    Data = newLesson,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<Lesson> GetLessonAsync(string id)
        {
            if (id == null) return null;

            var lesson = await dataContext.Lessons.Include(l => l.Post)
                .Include(l => l.Topic).ThenInclude(t => t.DisciplineTopic)
                .Include(l => l.Video)
                .Include(l => l.Article)
                .Include(l => l.TeacherPlace).ThenInclude(tp => tp.Discipline)
                .Include(l => l.TeacherPlace).ThenInclude(tp => tp.School)
                .SingleOrDefaultAsync(l => l.Id == id
                && l.Post.Situation == Constants.SituationsObjects.NormalSituation);

            return lesson;
        }

        public async Task<Lesson> GetLessonByPostAsync(string postId)
        {
            if (postId == null) return null;

            var lesson = await (from l in dataContext.Lessons
                                from p in dataContext.Posts
                                where l.PostId == postId && p.Id == postId && p.Situation == Constants.SituationsObjects.NormalSituation
                                select l).SingleOrDefaultAsync();

            return lesson;
        }

        public async Task<PageResult<Lesson>> GetLessonsAsync(PaginationFilter filter, LessonQuery query)
        {
            var lessons = dataContext.Lessons
                .Include(l => l.Post)
                .Include(l => l.Topic).ThenInclude(t => t.DisciplineTopic)
                .Include(l => l.Video)
                .Include(l => l.Article)
                .Include(l => l.TeacherPlace).ThenInclude(tp => tp.Discipline)
                .Include(l => l.TeacherPlace).ThenInclude(tp => tp.School)                
                .Where(l => l.Post.Situation == Constants.SituationsObjects.NormalSituation).AsQueryable();

            if (query != null)
            {
                if (query.TeacherId != null)
                {
                    lessons = from l in lessons
                              from tp in dataContext.TeacherPlaces
                              where tp.TeacherId == query.TeacherId && l.TeacherPlaceId == tp.Id
                              select l;
                }

                if (query?.SchoolId != null)
                    lessons = from l in lessons
                              from tp in dataContext.TeacherPlaces.AsQueryable()
                              where tp.SchoolId == query.SchoolId && l.TeacherPlaceId == tp.Id
                              select l;

                if (query?.TeacherPlaceId != null)
                    lessons = lessons.Where(l => l.TeacherPlaceId == query.TeacherPlaceId);

                if (query?.TopicId != null)
                    lessons = lessons.Where(l => l.TopicId == query.TopicId);
            }

            return await GetPaginationAsync(lessons, filter);
        }

        public async Task<bool> ObjectExists(string id)
        {
            return await dataContext.Lessons.AnyAsync(l => l.Id == id);
        }

        public async Task<int> QuestionCountAsync(string id)
        {
            return await dataContext.Questions.Where(q => q.LessonId == id).CountAsync();
        }

        public async Task<int> SolvedQuestionCountAsync(string id)
        {
            return await dataContext.Questions.Where(q => q.LessonId == id && q.Situation == QuestionSituations.Solved.ToString("g")).CountAsync();
        }

        public async Task<int> TeacherAnswerCountAsync(string id)
        {
            return await (from l in dataContext.Lessons
                          from a in dataContext.Answers.Include(a => a.Post)
                          from tp in dataContext.TeacherPlaces
                          from t in dataContext.Teachers
                          from q in dataContext.Questions
                          where l.Id == id && l.TeacherPlaceId == tp.Id && 
                          tp.TeacherId == t.Id && t.UserId == a.Post.UserId && 
                          q.Id == a.QuestionId && q.LessonId == id
                          select l).CountAsync();
        }

        public async Task<int> AnswerCountAsync(string id)
        {
            return await (from q in dataContext.Questions
                          from a in dataContext.Answers
                          where a.QuestionId == q.Id && q.LessonId == id
                          select a).CountAsync();
        }

        private async Task<PageResult<Lesson>> GetPaginationAsync(IQueryable<Lesson> lessons, PaginationFilter filter)
        {
            var searchMode = filter.SearchValue != null;
            var totalElement = await lessons.CountAsync();

            if (searchMode)
            {
                var sv = filter.SearchValue;

                lessons = lessons
                    .Where(l => l.Topic.DisciplineTopic.Name.Contains(sv) || l.Description.Contains(sv));
            }

            if (filter.PageNumber >= 0 || filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                lessons = lessons.Skip(skip).Take(filter.PageSize);
            }

            var data = await lessons.OrderBy(l => l.Sequence).ToListAsync();

            var page = new PageResult<Lesson>
            {
                Data = data,
                TotalElements = totalElement
            };

            return page;
        }

        private CreationResult<Lesson> FailCreation()
        {
            return new CreationResult<Lesson>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private async Task<int> BuildLessonSequence(string topicId)
        {
            var length = await (from t in dataContext.Topics.Include(t => t.Post)
                                from l in dataContext.Lessons
                                where l.TopicId == topicId && t.Id == topicId &&
                                t.Post.Situation == Constants.SituationsObjects.NormalSituation
                                select l).CountAsync();

            return length + 1;
        }
    }
}
