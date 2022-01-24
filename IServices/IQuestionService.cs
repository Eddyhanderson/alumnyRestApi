using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using Alumni.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IQuestionService
    {
        Task<CreationResult<Question>> CreateAsync(Question question);

        Task<Question> GetQuestionAsync(string id);

        Task<int> GetStudantAnswerQntAsync(string questionId);

        Task<int> GetTeacherAnswerQntAsync(string questionId);

        Task<int> GetCommentsQntAsync(string questionId);

        Task<PageResult<Question>> GetQuestionsAsync(PaginationFilter filter = null, QuestionQuery questionQuery = null);

        Task<Question> PatchQuestionAsync(Question question);

        Task<bool> ObjectExists(string id);
    }
}
