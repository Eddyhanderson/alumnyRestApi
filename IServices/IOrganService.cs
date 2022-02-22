using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IOrganService
    {
        Task<CreationResult<Organ>> CreateAsync(Organ organ);

        Task<PageResult<Organ>> GetOrgansAsync(PaginationFilter filter = null, OrganQuery query = null);
    }
}
