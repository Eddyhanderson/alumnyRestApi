using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface ITeacherService
    {
        Task<CreationResult<Teacher>> CreateAsync(Teacher teacher);

        Task<PageResult<Teacher>> GetTeachersAsync(PaginationFilter filter = null);

        Task<Teacher> GetTeacherAsync(string id);

        Task<int> TeacherPlaceCountAsync(string teacherId);

        Task<Teacher> GetTeacherByLessonAsync(string lessonId);

        Task<bool> ObjectExists(string id);        
    }
}