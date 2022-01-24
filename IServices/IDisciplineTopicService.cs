using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IDisciplineTopicService
    {
        Task<CreationResult<DisciplineTopic>> CreateAsync(DisciplineTopic disciplineTopic);
        
        Task<DisciplineTopic> GetDisciplineTopicAsync(string disciplineTopicId);
        
        Task<IEnumerable<DisciplineTopic>> GetDisciplineTopicsAsync(PaginationFilter filter = null, DisciplineTopicQuery disciplineTopicQuery = null);

        Task<bool> ObjectExists(string id);
    }
}
