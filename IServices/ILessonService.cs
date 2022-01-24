using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using Alumni.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface ILessonService
    {
        Task<CreationResult<Lesson>> CreateAsync(Lesson lesson);

        Task<Lesson> GetLessonAsync(string id);
     
        Task<PageResult<Lesson>> GetLessonsAsync(PaginationFilter filter, LessonQuery query = null);        

        Task<bool> ObjectExists(string id);

        Task<int> QuestionCountAsync(string id);

        Task<int> SolvedQuestionCountAsync(string id);

        Task<int> TeacherAnswerCountAsync(string id);

        Task<int> AnswerCountAsync(string id);
    }
}
