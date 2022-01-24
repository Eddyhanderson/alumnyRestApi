using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface ITopicService
    {
        Task<CreationResult<Topic>> CreateAsync(Topic topic);

        Task<Topic> GetTopicAsync(string topicId);

        Task<IEnumerable<Topic>> GetTopicsAsync(PaginationFilter filter = null, TopicQuery topicQuery = null);

        Task<int> LessonCountAsync(string topicId);

        Task<int> OpenLessonCountAsync(string topicId);

        Task<int> QuestionCountAsync(string topicId);

        Task<int> SolvedQuestionCountAsync(string topicId);

        Task<int> AnswerCountAsync(string topicId);

        Task<int> CommentCountAsync(string topicId);

        Task<int> StudantAnswerCount(string topicId);

        Task<int> TeacherAnswerCount(string topicId);
    }
}
