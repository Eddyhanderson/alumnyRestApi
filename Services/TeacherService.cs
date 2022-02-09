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
    public class TeacherService : ITeacherService
    {
        private readonly DataContext dataContext;

        private readonly IUserService userService;

        public TeacherService(DataContext dataContext, IUserService userService)
        {
            this.dataContext = dataContext;

            this.userService = userService;
        }

        public async Task<CreationResult<Teacher>> CreateAsync(Teacher teacher)
        {
            if (teacher == null) return null;

            var teacherCode = await GenerateTeacherCodeAsync(teacher);

            var newTeacher = new Teacher
            {
                Id = Guid.NewGuid().ToString(),
                TeacherCode = teacherCode,
                UserId = teacher.UserId,
                AcademicLevelId = teacher.AcademicLevelId,
                AcademyId = teacher.AcademyId,
                CourseId = teacher.CourseId
            };

            await dataContext.Teachers.AddAsync(newTeacher);

            try
            {
                await dataContext.SaveChangesAsync();

                return new CreationResult<Teacher>
                {
                    Data = newTeacher,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        // TODO: Change the teacher query for teacher Id
        public async Task<Teacher> GetTeacherAsync(string id)
        {
            if (id == null) return null;

            var teacher = await dataContext.Teachers
                .Include(t => t.AcademicLevel)
                .Include(t => t.Academy)
                .Include(t => t.Course)
                .Include(t => t.User)
                .SingleOrDefaultAsync(t => t.Id == id);

            return teacher;
        }

        public async Task<PageResult<Teacher>> GetTeachersAsync(PaginationFilter filter = null)
        {
            var teachers = dataContext.Teachers
                           .Include(t => t.AcademicLevel)
                           .Include(t => t.Academy)
                           .Include(t => t.Course)
                           .Include(t => t.User)
                           .Where(t => t.User.Situation == Constants.SituationsObjects.NormalSituation);

            var totalElement = await teachers.CountAsync();

            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                /*teachers = teachers.Include(t => t.User)
                    .Where(t => t.User.FirstName.Contains(sv) || t.User.LastName.Contains(sv)
                    || t.TeacherCode.Contains(sv));*/
            }

            if (filter.PageNumber >= 0 || filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                teachers = teachers.Skip(skip).Take(filter.PageSize);
            }

            var pageResult = new PageResult<Teacher>
            {
                Data = await teachers.ToListAsync(),
                TotalElements = totalElement
            };

            return pageResult;
        }

        public async Task<bool> ObjectExists(string id)
        {
            return await dataContext.Teachers.AnyAsync(t => t.Id == id);
        }

        public async Task<int> TeacherPlaceCountAsync(string teacherId)
        {
            return await dataContext.TeacherPlaces.Where(tp => tp.TeacherId == teacherId).CountAsync();
        }

        public async Task<Teacher> GetTeacherByLessonAsync(string lessonId)
        {
            if (lessonId == null) return null;

            var teacher = await (from t in dataContext.Teachers
                                 from tp in dataContext.TeacherPlaces
                                 from l in dataContext.Lessons
                                 where t.Id == tp.TeacherId && l.TeacherPlaceId == tp.Id && l.Id == lessonId
                                 select t).SingleOrDefaultAsync();

            return teacher;
        }

        private CreationResult<Teacher> FailCreation()
        {
            return new CreationResult<Teacher>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private async Task<string> GenerateTeacherCodeAsync(Teacher teacher)
        {
            var user = await userService.GetUserAsync(teacher.UserId);

            /*var prefix = string.Concat(user.FirstName[0], user.LastName[0]);*/

            var seq = await dataContext.Teachers.CountAsync() + 1;

            var year = DateTime.Today.Year;

            /*return string.Concat(prefix, year, seq);*/
            return "";
        }
    }
}






