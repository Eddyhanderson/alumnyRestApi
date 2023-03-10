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

        Task<PageResult<Formation>> GetPublishedFormationsAsync(PaginationFilter filter = null, FormationQuery query = null);

        Task<PageResult<Formation>> GetSubscribedFormationsAsync(PaginationFilter filter = null, FormationQuery query = null);

        Task<PageResult<Formation>> GetFinishedFormationsAsync(PaginationFilter filter = null, FormationQuery query = null);
        Task<Formation> GetFormationAsync(string id);
    }
}