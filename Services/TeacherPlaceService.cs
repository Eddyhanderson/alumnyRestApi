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
    public class TeacherPlaceService : ITeacherPlaceService
    {
        private readonly DataContext dataContext;

        private readonly ITeacherService teacherService;

        private readonly IDisciplineService disciplineService;

        private readonly ITeacherPlaceStudantService teacherPlaceStudantService;

        public TeacherPlaceService(DataContext dataContext,
            ITeacherService teacherService,
            IDisciplineService disciplineService,
            ITeacherPlaceStudantService teacherPlaceStudantService)
        {
            this.dataContext = dataContext;

            this.teacherService = teacherService;

            this.disciplineService = disciplineService;

            this.teacherPlaceStudantService = teacherPlaceStudantService;
        }
        

        // ========================================================================================= //

        public async Task<CreationResult<TeacherPlace>> CreateAsync(TeacherPlace teacherPlace)
        {
            if (teacherPlace == null) return null;

            var code = await GenerateTeacherCodeAsync(teacherPlace);

            var exists =  await dataContext.TeacherPlaces
                .AnyAsync(tp => tp.TeacherPlaceCode == code);

            if (exists) return FailCreation();

            try
            {
                var newTeacherPlace = new TeacherPlace
                {
                    Id = Guid.NewGuid().ToString(),
                    CourseId = teacherPlace.CourseId,
                    SchoolId = teacherPlace.SchoolId,
                    DisciplineId = teacherPlace.DisciplineId,
                    Name = teacherPlace.Name,
                    ProfilePhotoPath = teacherPlace.ProfilePhotoPath,
                    TeacherId = teacherPlace.TeacherId,
                    TeacherPlaceCode = code,
                    Description = teacherPlace.Description,
                    Opened = teacherPlace.Opened,
                    Situation = Constants.SituationsObjects.NormalSituation
                };

                await dataContext.TeacherPlaces.AddAsync(newTeacherPlace);

                await dataContext.SaveChangesAsync();

                return new CreationResult<TeacherPlace>
                {
                    Data = newTeacherPlace,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }

        }

        public async Task<TeacherPlace> GetTeacherPlaceAsync(string id)
        {
            if (id == null) return null;

            var teacherPlace =  await dataContext.TeacherPlaces
                                .Include(tp => tp.Teacher).ThenInclude(t => t.User)
                                .Include(tp => tp.Course)
                                /*.Include(tp => tp.School).ThenInclude(s => s.BadgeInformation)*/
                                .Include(tp => tp.Discipline)
                .SingleOrDefaultAsync(s => s.Id == id && s.Situation == Constants.SituationsObjects.NormalSituation);

            return teacherPlace;
        }

        public async Task<IEnumerable<TeacherPlace>> GetTeacherPlacesAsync(PaginationFilter filter = null, 
            TeacherPlaceQuery query = null)
        {

            var teacherPlaces = from tp in dataContext.TeacherPlaces
                                 .Include(tp => tp.Teacher).ThenInclude(t => t.User)
                                 .Include(tp => tp.Course)
                                 /*.Include(tp => tp.School).ThenInclude(s => s.BadgeInformation)*/
                                 .Include(tp => tp.Discipline)
                                select tp;

            ApplyQueries(ref teacherPlaces, query);

            return await GetPaginationAsync(teacherPlaces, filter);
        }

        public async Task<int>  StudantsCountAsync(string teacherPlaceId)
        {
            return await dataContext.TeacherPlaceStudants
                .Where(tps => tps.TeacherPlaceId == teacherPlaceId &&
                tps.Situation == TeacherPlaceRegistrationState.Registered.ToString())
                .CountAsync();
        }

        public async Task<int>  LessonsCountAsync(string teacherPlaceId)
        {
            return await dataContext.Lessons
                .Include(l => l.Post)
                .Where(l => l.ModuleId == teacherPlaceId &&
                l.Post.Situation == Constants.SituationsObjects.NormalSituation).CountAsync();
        }

        public async Task<int>  TopicCountAsync(string teacherPlaceId)
        {
            return await dataContext.Topics.Where(t => t.TeacherPlaceId == teacherPlaceId
            && t.Post.Situation == Constants.SituationsObjects.NormalSituation)
                .CountAsync();
        }

        public async Task<int>  QuestionsCountAsync(string teacherPlaceId)
        {
            return await (from q in dataContext.Questions
                          from l in dataContext.Lessons
                          where q.LessonId == l.Id && l.ModuleId == teacherPlaceId
                          select q).CountAsync();
        }

        public async Task<int>  AnswerCountAsync(string teacherPlaceId)
        {
            return await (from a in dataContext.Answers
                          from l in dataContext.Lessons
                          from q in dataContext.Questions
                          where a.QuestionId == q.Id && q.LessonId == l.Id && l.ModuleId == teacherPlaceId
                          select a).CountAsync();
        }

        public async Task<int>  StudantAnswerCountAsync(string teacherPlaceId)
        {
            return await (from s in dataContext.Studants
                          from a in dataContext.Answers
                          from l in dataContext.Lessons
                          from q in dataContext.Questions
                          where a.QuestionId == q.Id && q.LessonId == l.Id && l.ModuleId == teacherPlaceId
                          && a.Post.UserId == s.UserId
                          select a).CountAsync();
        }

        public async Task<int>  TeacherAnswerCountAsync(string teacherPlaceId)
        {
            return await (from q in dataContext.Questions
                                from l in dataContext.Lessons
                                from a in dataContext.Answers
                                from tp in dataContext.TeacherPlaces
                                from t in dataContext.Teachers
                                where q.LessonId == l.Id && l.ModuleId == teacherPlaceId && a.QuestionId == q.Id
                                && a.Post.UserId == t.UserId && t.Id == tp.TeacherId && tp.Id == teacherPlaceId
                                select a).CountAsync();
        }

        public async Task<int>  SolvedQuestionCountAsync(string teacherPlaceId)
        {
            return await (from q in dataContext.Questions
                          from l in dataContext.Lessons
                          where q.LessonId == l.Id && l.ModuleId == teacherPlaceId && 
                          q.Situation == QuestionSituations.Solved.ToString("g")
                          select q).CountAsync();
        }

        public async Task<bool> CheckIfTeacherPlaceExists(string id)
        {
            return await dataContext.TeacherPlaces.FindAsync(id) != null;
        }

        private async Task<IEnumerable<TeacherPlace>> GetPaginationAsync(IQueryable<TeacherPlace> teacherPlaces,
            PaginationFilter filter)
        {
            if (filter == null) return await teacherPlaces.ToListAsync();

            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                teacherPlaces = teacherPlaces
                    .Where(tp => tp.Name.Contains(sv) || tp.TeacherPlaceCode.Contains(sv));
            }

            if (filter.PageNumber >= 0 && filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                teacherPlaces = teacherPlaces.Skip(skip).Take(filter.PageSize);
            }

            return await teacherPlaces.ToListAsync();
        }

        private CreationResult<TeacherPlace> FailCreation()
        {
            return new CreationResult<TeacherPlace>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private async Task<string> GenerateTeacherCodeAsync(TeacherPlace teacherPlace)
        {
            var nameCode = string.Concat(teacherPlace.Name.ElementAt(0), teacherPlace.Name.ElementAt(teacherPlace.Name.Length - 1));

            var seq =  await dataContext.TeacherPlaces.CountAsync() + 1;

            var year = DateTime.UtcNow.Year;

            return string.Concat(nameCode, seq, year);
        }

        private async Task<bool> VerifyIfTeacherPlaceExists(string code)
        {
            return await dataContext.TeacherPlaces
                .SingleOrDefaultAsync(tp => tp.TeacherPlaceCode == code) != null;
        }       

        private void ApplyQueries(ref IQueryable<TeacherPlace> teacherPlaces, TeacherPlaceQuery query)
        {
            if (query?.Opened == "1" || query?.Opened == "0")
            {
                var opened = query.Opened == "1";
                teacherPlaces = teacherPlaces.Where(tp => tp.Opened == opened);
            }

            if (query.TeacherId != null)
                teacherPlaces = teacherPlaces.Where(tp => tp.TeacherId == query.TeacherId);

            if (query.SchoolId != null)
                teacherPlaces = teacherPlaces.Where(tp => tp.SchoolId == query.SchoolId);

            if (query.CourseId != null)
                teacherPlaces = teacherPlaces.Where(tp => tp.CourseId == query.CourseId);

            if (query.StudantId != null)
                teacherPlaces = from tp in teacherPlaces
                                from tps in dataContext.TeacherPlaceStudants
                                where tps.StudantId == query.StudantId && tps.TeacherPlaceId == tp.Id
                                select tp;
        }

        public Task<TeacherPlace> UpdateAsync(TeacherPlace entity, string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TeacherPlace entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TeacherPlace>> GetTeacherPlacesByStudantAsync(string studantId, PaginationFilter filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TeacherPlace>> GetTeacherPlacesOfTeacherBySchoolAsync(string teacherId, string schoolId, PaginationFilter filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TeacherPlaceExists(string id)
        {
            throw new NotImplementedException();
        }
    }
}
