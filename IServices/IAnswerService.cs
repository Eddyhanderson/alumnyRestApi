using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IAnswerService
    {
        public Task<CreationResult<Answer>> CreateAsync(Answer answer);
        public Task<Answer> GetAsync(string id);
        public Task<PageResult<Answer>> GetAllAsync(PaginationFilter filter, AnswerQuery query);
    }
}
