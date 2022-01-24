using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface ICourseService
    {
        Task<CreationResult<Course>> CreateAsync(Course course);

        Task<IEnumerable<Course>> GetCoursesAsync(PaginationFilter filter = null, CourseQuery param = null);

        Task<Course> GetCourseAsync(CourseQuery query);
    }
}
