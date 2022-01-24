using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface ITeacherPlaceService
    {
        Task<CreationResult<TeacherPlace>> CreateAsync(TeacherPlace teacherPlace);

        Task<TeacherPlace> GetTeacherPlaceAsync(string id);

        Task<IEnumerable<TeacherPlace>> GetTeacherPlacesByStudantAsync(string studantId, PaginationFilter filter = null);

        Task<IEnumerable<TeacherPlace>> GetTeacherPlacesAsync(PaginationFilter filter = null, TeacherPlaceQuery param = null);

        Task<IEnumerable<TeacherPlace>> GetTeacherPlacesOfTeacherBySchoolAsync(string teacherId, string schoolId, PaginationFilter filter = null);

        Task<int> AnswerCountAsync(string teacherPlaceId);

        Task<int> StudantsCountAsync(string teacherPlaceId);

        Task<int> LessonsCountAsync(string teacherPlaceId);

        Task<int> TopicCountAsync(string teacherPlaceId);

        Task<int> QuestionsCountAsync(string teacherPlaceId);

        Task<int> StudantAnswerCountAsync(string teacherPlaceId);

        Task<int> TeacherAnswerCountAsync(string teacherPlaceId);

        Task<int> SolvedQuestionCountAsync(string teacherPlaceId);

        Task<bool> TeacherPlaceExists(string id);
    }
}
