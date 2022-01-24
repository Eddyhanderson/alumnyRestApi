using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IAcademyService
    {
        Task<CreationResult<Academy>> CreationAsync(Academy academy);

        Task<Academy> GetAsync(AcademyQuery query);

        Task<IEnumerable<Academy>> GetAllAsync(PaginationFilter filter);
    }
}
