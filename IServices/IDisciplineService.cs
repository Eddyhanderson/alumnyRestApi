using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IDisciplineService
    {
        Task<CreationResult<Discipline>> CreateAsync(Discipline discipline);

        Task<Discipline> GetDisciplineAsync(string disciplineId);

        Task<IEnumerable<Discipline>> GetDisciplinesAsync(PaginationFilter filter = null);

        public Task<bool> ObjectExists(string id);
    }
}
