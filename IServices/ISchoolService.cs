using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface ISchoolService
    {
        Task<CreationResult<School>> CreateAsync(School school);

        Task<IEnumerable<School>> GetSchoolsAsync(PaginationFilter filter = null, SchoolQuery schoolQuery = null);

        Task<School> GetSchoolAsync(string id);
    }
}
