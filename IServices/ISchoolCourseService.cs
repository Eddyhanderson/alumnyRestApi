using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface ISchoolCourseService
    {
        Task<IEnumerable<SchoolCourses>> GetSchoolCoursesAsync(PaginationFilter filter = null, SchoolCourseQuery param = null);
        
        Task<CreationResult<SchoolCourses>> CreateAsync(SchoolCourses schoolCourses);
    }
}
