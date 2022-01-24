using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface ITeacherSchoolsService
    {
        Task<CreationResult<TeacherSchools>> CreateAsync(TeacherSchools teacherSchools);

        Task<bool> UpdateAsync(string teacherId, string schoolId, TeacherSchools teacherSchools);

        Task<IEnumerable<TeacherSchools>> GetAllAsync(PaginationFilter filter, TeacherSchoolQuery query);

        Task<bool> CheckTeacherHasSchoolAsync(string teacherId);
    }
}
