using System.Collections.Generic;
using System.Threading.Tasks;
using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;

namespace alumni.IServices
{
    public interface IFormationService
    {
        Task<CreationResult<Formation>> CreateAsync(Formation formation);
        Task<PageResult<Formation>> GetFormationsAsync(PaginationFilter filter = null, FormationQuery query = null);
    }
}