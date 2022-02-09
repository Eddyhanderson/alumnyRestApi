using alumni.Contracts.V1.Requests.Queries;
using alumni.Data;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly DataContext dataContext;

        private readonly IPostService postService;

        private readonly UserManager<User> userManager;

        public QuestionService(DataContext dataContext,
            IPostService postService,
            UserManager<User> userManager)
        {
            this.dataContext = dataContext;

            this.postService = postService;

            this.userManager = userManager;
        }

        public async Task<CreationResult<Question>> CreateAsync(Question question)
        {
            if (question == null) return null;


            var postStt = await postService.CreateAsync(PostsTypes.Question);

            if (!postStt.Succeded) FailCreation();

            question.PostId = postStt.Data.Id;

            try
            {
                var newQuestion = new Question
                {
                    Id = Guid.NewGuid().ToString(),
                    Content = question.Content,
                    LessonId = question.LessonId,
                    PostId = question.PostId,
                    Subject = question.Subject,
                    StudantId = question.StudantId,
                    Situation = QuestionSituations.Waiting.ToString("g")
                };

                await dataContext.Questions.AddAsync(newQuestion);

                await dataContext.SaveChangesAsync();

                return new CreationResult<Question>
                {
                    Data = newQuestion,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<PageResult<Question>> GetQuestionsAsync(PaginationFilter filter = null, QuestionQuery questionQuery = null)
        {
            var normalState = Constants.SituationsObjects.NormalSituation;
            var situationFiltered = questionQuery?.Situation.ToString("g") != QuestionSituations.All.ToString("g");

            // if are researching for questions aimed at the teacher, specifying or not any studant
            if (questionQuery.TeacherId != null)
            {
                var _questions = from q in dataContext.Questions
                                 .Include(q => q.Post)
                                 .Include(q => q.Lesson)
                                 .Include(q => q.Studant).ThenInclude(s => s.User)
                                 from l in dataContext.Lessons
                                 from tp in dataContext.TeacherPlaces
                                 where q.LessonId == l.Id && l.TeacherPlaceId == tp.Id && tp.TeacherId == questionQuery.TeacherId
                                       && q.Post.Situation == normalState
                                 select q;

                // if only studant question are requested
                if (questionQuery.StudantId != null)
                {
                    _questions = _questions.Where(q => q.StudantId == questionQuery.StudantId);
                }

                // if only specific question situation are requested
                if (situationFiltered)
                    _questions = _questions.Where(q => q.Situation == questionQuery.Situation.ToString("g"));

                return await GetPaginationAsync(_questions, filter);
            }

            // If you are researching for studant questions specifying or not any teacher place and any lesson
            if (questionQuery.StudantId != null && questionQuery.TeacherId == null)
            {
                var _questions = from q in dataContext.Questions
                                 .Include(q => q.Post)
                                 .Include(q => q.Studant).ThenInclude(s => s.User)
                                 from l in dataContext.Lessons
                                 where q.StudantId == questionQuery.StudantId
                                       && q.Post.Situation == normalState
                                 select q;

                // if only specific teacher place are requested
                if (questionQuery.TeacherPlaceId != null)
                {
                    _questions = from l in dataContext.Lessons
                                 from q in _questions
                                 .Include(q => q.Post)
                                 .Include(q => q.Studant).ThenInclude(s => s.User)
                                 where l.Id == q.LessonId && l.TeacherPlaceId == questionQuery.TeacherPlaceId
                                 select q;
                }

                // if only specific lesson are requested
                if (questionQuery.LessonId != null)
                {
                    _questions = from q in _questions
                                 .Include(q => q.Post)
                                 .Include(q => q.Studant).ThenInclude(s => s.User)
                                 where q.LessonId == questionQuery.LessonId
                                 select q;
                }

                if (situationFiltered)
                    _questions = _questions.Where(q => q.Situation == questionQuery.Situation.ToString("g"));

                return await GetPaginationAsync(_questions, filter);
            }

            // If you are researching for studant questions aimed at the teacher place
            if (questionQuery.TeacherPlaceId != null && questionQuery.StudantId == null)
            {
                var _questions = from l in dataContext.Lessons
                                 from q in dataContext.Questions.Include(q => q.Post)
                                 .Include(q => q.Post)
                                 .Include(q => q.Studant).ThenInclude(s => s.User)
                                 where q.LessonId == l.Id && l.TeacherPlaceId == questionQuery.TeacherPlaceId && q.Post.Situation == normalState
                                 select q;


                if (situationFiltered)
                    _questions = _questions.Where(q => q.Situation == questionQuery.Situation.ToString("g"));

                return await GetPaginationAsync(_questions, filter);

            }

            // If you are researching for studant questions aimed at the lesson
            if (questionQuery.LessonId != null && questionQuery.StudantId == null)
            {
                var _questions = from q in dataContext.Questions.Include(q => q.Post)
                                 .Include(q => q.Post)
                                 .Include(q => q.Studant).ThenInclude(s => s.User)
                                 where q.LessonId == questionQuery.LessonId && q.Post.Situation == normalState
                                 select q;

                if (situationFiltered)
                    _questions = _questions.Where(q => q.Situation == questionQuery.Situation.ToString("g"));

                return await GetPaginationAsync(_questions, filter);

            }

            return null;
        }

        public async Task<Question> GetQuestionAsync(string id)
        {
            if (id == null) return null;

            var question = await dataContext.Questions
                .Include(q => q.Post)
                .Include(q => q.Studant).ThenInclude(s => s.User)
                .Include(q => q.Lesson)
                .SingleOrDefaultAsync(q => q.Id == id && q.Post.Situation == Constants.SituationsObjects.NormalSituation);

            return question;
        }

        public Task<bool> ObjectExists(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetStudantAnswerQntAsync(string questionId)
        {
            var qnt = await
                (from ur in dataContext.UserRoles
                 from r in dataContext.Roles
                 from a in dataContext.Answers
                 .Include(a => a.Post).ThenInclude(p => p.User)
                 .Where(a => a.QuestionId == questionId &&
                 a.Post.UserId == ur.UserId && ur.RoleId == r.Id && r.NormalizedName == Constants.UserContansts.StudantRole.ToUpper())
                 select a.Id).CountAsync();

            return qnt;

        }

        public async Task<int> GetTeacherAnswerQntAsync(string questionId)
        {
            var qnt = await
                (from ur in dataContext.UserRoles
                 from r in dataContext.Roles
                 from l in dataContext.Lessons
                 from tp in dataContext.TeacherPlaces
                 from t in dataContext.Teachers
                 from q in dataContext.Questions
                 from a in dataContext.Answers
                 .Include(a => a.Post).ThenInclude(p => p.User)
                 .Where(a => a.QuestionId == questionId &&
                 a.Post.UserId == ur.UserId && ur.RoleId == r.Id &&
                 r.NormalizedName == Constants.UserContansts.TeacherRole.ToUpper() &&
                 q.Id == questionId && q.LessonId == l.Id && l.TeacherPlaceId == tp.Id
                 && tp.TeacherId == t.Id && t.UserId == ur.UserId)
                 select a.Id).CountAsync();

            return qnt;
        }

        public async Task<int> GetCommentsQntAsync(string questionId)
        {
            var qnt = await (from c in dataContext.Comments
                 .Include(c => c.Post)
                             from q in dataContext.Questions
                             .Where(q => q.Id == questionId && q.PostId == c.PostId && c.Post.Situation == Constants.SituationsObjects.NormalSituation)
                             select q.Id).CountAsync();

            return qnt;
        }

        public async Task<Question> PatchQuestionAsync(Question question)
        {
            try
            {
                dataContext.Entry(question).State = EntityState.Modified;
                var stt = await dataContext.SaveChangesAsync();

                if (stt > 0)
                    return question;

                return null;

            }
            catch (DbUpdateConcurrencyException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        private async Task<PageResult<Question>> GetPaginationAsync(IQueryable<Question> questions, PaginationFilter filter)
        {
            var searchMode = filter.SearchValue != null;
            var totalElement = await questions.CountAsync();

            if (searchMode)
            {
                var sv = filter.SearchValue;

                questions = questions
                    .Where(q => q.Lesson.Title.Contains(filter.SearchValue)
               /*     || q.Studant.User.FirstName.Contains(filter.SearchValue)
                    || q.Studant.User.LastName.Contains(filter.SearchValue)*/
                    || q.Content.Contains(filter.SearchValue)
                    || q.Subject.Contains(filter.SearchValue));
            }

            if (filter.PageNumber >= 0 || filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                questions = questions.Skip(skip).Take(filter.PageSize);
            }

            var data = await questions.OrderByDescending(l => l.Post.CreateAt).ToListAsync();

            var page = new PageResult<Question>
            {
                Data = data,
                TotalElements = totalElement
            };

            return page;
        }

        private CreationResult<Question> FailCreation()
        {
            return new CreationResult<Question>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }
    }
}