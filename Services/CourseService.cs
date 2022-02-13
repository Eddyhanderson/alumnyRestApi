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
    public class CourseService : ICourseService
    {
        private readonly DataContext dataContext;

        private readonly IBadgeInformationService badgeInformationService;

        public CourseService(DataContext dataContext, IBadgeInformationService badgeInformationService)
        {
            this.dataContext = dataContext;

            this.badgeInformationService = badgeInformationService;
        }

        public async Task<CreationResult<Course>> CreateAsync(Course course)
        {
            if (course == null) return FailCreation();

            var exists = await dataContext.Courses
                .AnyAsync(c => c.Name.ToUpper() == course.Name.ToUpper());

            if (exists) return FailCreation();

            try
            {
                if (course.BadgeInformationId == null)
                {
                    var stt = await badgeInformationService.CreateAsync(course.BadgeInformation);

                    if (!stt.Succeded) return FailCreation();

                    course.BadgeInformationId = stt.Data.Id;
                }

                var newCourse = new Course
                {
                    Id = Guid.NewGuid().ToString(),                    
                    BadgeInformationId = course.BadgeInformationId,
                    Name = course.Name,                    
                };

                await dataContext.Courses.AddAsync(newCourse);

                await dataContext.SaveChangesAsync();

                return new CreationResult<Course>
                {
                    Data = newCourse,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<Course> GetCourseAsync(CourseQuery query)
        {
            if (query.Id == null && query.Name == null) return null;
            if (query.Id != null && query.Name != null) return null;

            if(query.Id != null)
            {
            return await dataContext
                .Courses.SingleOrDefaultAsync(c => c.Id == query.Id);
            }

            if (query.Name != null)
            {
                return await dataContext
                    .Courses.SingleOrDefaultAsync(c => c.Name == query.Name);
            }
            return null;
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync(PaginationFilter filter = null, CourseQuery param = null)
        {
            var courses = from c in dataContext.Courses.Include(c => c.BadgeInformation)
                          where c.BadgeInformation.Situation == Constants.SituationsObjects.NormalSituation                                
                          select c;

            if(param != null && param.SchoolId != null)
            {
                /*courses = dataContext.SchoolCourses
                    .Include(sc => sc.Course)
                    .Where(sc => sc.SchoolId == param.SchoolId)
                    .Select(sc => sc.Course);*/
            }

            if (filter == null) return await courses.ToListAsync();

            if (filter.SearchValue != null)
            {
                var sv = filter.SearchValue;

                courses = courses.Where(c => c.Name.Contains(sv));
            }

            if (filter.PageNumber >= 0 && filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                courses = courses.Skip(skip).Take(filter.PageSize);
            }

            return courses;
        }        
       
        private CreationResult<Course> FailCreation()
        {
            return new CreationResult<Course>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }
    }
}
