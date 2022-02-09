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
    public class SchoolCourseService : ISchoolCourseService
    {
        private DataContext dataContext;

        public SchoolCourseService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<CreationResult<SchoolCourses>> CreateAsync(SchoolCourses schoolCourses)
        {
            if (schoolCourses == null || schoolCourses.CourseId == null || schoolCourses.SchoolId == null) return null;

            schoolCourses.Situation = Constants.SituationsObjects.NormalSituation;

            try
            {
                await dataContext.SchoolCourses.AddAsync(schoolCourses);

                var stt = await dataContext.SaveChangesAsync();

                return new CreationResult<SchoolCourses>
                {
                    Data = schoolCourses,
                    Succeded = true,
                    Exists = false
                };
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (await SchoolCourseExists(schoolCourses))
                    return DataExist();

                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<IEnumerable<SchoolCourses>> GetSchoolCoursesAsync(PaginationFilter filter = null, SchoolCourseQuery param = null)
        {
            if (param != null && param.CourseId != null && param.SchoolId != null) return null;

            var searchMode = filter.SearchValue != null;

            var schoolCourses = dataContext.SchoolCourses.AsQueryable();

            if (param.Situation != null)
                schoolCourses = schoolCourses.Where(sc => sc.Situation.ToUpper() == param.Situation.ToUpper());

            if (param.CourseId != null)
                schoolCourses = schoolCourses
                /*    .Include(sc => sc.School).ThenInclude(s => s.BadgeInformation)*/
                    .Where(sc => sc.CourseId == param.CourseId);


            if (param.SchoolId != null)
                schoolCourses = schoolCourses
                    .Include(sc => sc.Course).ThenInclude(c => c.BadgeInformation)
                    .Where(sc => sc.SchoolId == param.SchoolId);

            /*if (searchMode)
                schoolCourses = schoolCourses.Where(sc => sc.Course.Name.Contains(filter.SearchValue) ||
                                                sc.School.Name.Contains(filter.SearchValue));*/


            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return await schoolCourses.ToListAsync();

            var skip = (filter.PageNumber - 1) * filter.PageSize;

            return await schoolCourses.Skip(skip).Take(filter.PageSize).ToListAsync();
        }

        private async Task<bool> SchoolCourseExists(SchoolCourses scm)
        {
            var exists = await dataContext.SchoolCourses.AnyAsync(sc => sc.CourseId == scm.CourseId && sc.SchoolId == scm.SchoolId && sc.Situation == Constants.SituationsObjects.NormalSituation);

            return exists;
        }

        private CreationResult<SchoolCourses> FailCreation()
        {
            return new CreationResult<SchoolCourses>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private CreationResult<SchoolCourses> DataExist()
        {
            return new CreationResult<SchoolCourses>
            {
                Succeded = false,
                Exists = true,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }
    }
}
