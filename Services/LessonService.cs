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
            if (lesson == null) return FailCreation();
            if (lesson.ModuleId is null) return FailCreation();

            try
            {
                CreationResult<Post> postStt = new CreationResult<Post>();

                if (lesson.PostId == null)
                {
                    postStt = await postService.CreateAsync(PostsTypes.Lesson);

                    if (!postStt.Succeded) return FailCreation();

                    lesson.PostId = postStt.Data.Id;
                }


                if (lesson.ArticleId == null && lesson.VideoId == null)
                    return FailCreation();

                if (lesson.LessonType != LessonTypes.Video.ToString("g") && lesson.LessonType != LessonTypes.Article.ToString("g"))
                {
                    return FailCreation();
                }

                if (lesson.LessonType == LessonTypes.Article.ToString("g"))
                {
                    var article = dataContext.Articles.FirstOrDefault(l => l.Id == lesson.ArticleId);

                    if (article == null) return FailCreation();

                    article.Draft = false;


                    var artStt = await articleService.UpdateAsync(lesson.ArticleId, article);

                    if (artStt == null) return FailCreation();
                }

                var newLesson = new Lesson
                {
                    Id = Guid.NewGuid().ToString(),
                    PostId = lesson.PostId,
                    Picture = lesson.Picture,
                    Description = lesson.Description,
                    Sequence = await BuildLessonSequence(lesson.ModuleId),
                    Views = 0,
                    ModuleId = lesson.ModuleId,
                    Title = lesson.Title,
                    ArticleId = lesson.ArticleId,
                    VideoId = lesson.VideoId,
                    LessonType = lesson.LessonType,
                    ManifestPath = lesson.ManifestPath
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
                .Include(l => l.Module).ThenInclude(m => m.Formation).ThenInclude(f => f.School).ThenInclude(s => s.User)
                .Include(l => l.Video)
                .Include(l => l.Article)
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
                .Include(l => l.Module).ThenInclude(m => m.Formation).ThenInclude(f => f.School)
                .Include(l => l.Video)
                .Include(l => l.Article)
                .Where(l => l.Post.Situation == Constants.SituationsObjects.NormalSituation).AsQueryable();

            if (query != null)
            {
                if (query?.SchoolId != null)
                    lessons = lessons.Where(l => l.Module.Formation.SchoolId == query.SchoolId);

                if (query?.ModuleId != null)
                    lessons = lessons.Where(l => l.ModuleId == query.ModuleId);

                if (query.FormationId != null)
                    lessons = lessons.Where(l => l.Module.FormationId == query.FormationId);
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
                    .Where(l => l.Title.Contains(sv) || l.Description.Contains(sv));
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

        private async Task<int> BuildLessonSequence(string moduleId)
        {
            var length = await dataContext.Lessons.Where(l => l.ModuleId == moduleId).CountAsync();

            return length + 1;
        }
    }
}
