using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IStudantService
    {
        public Task<CreationResult<Studant>> CreateAsync(Studant studant, User user, AuthData auth);

        public Task<Studant> GetStudantAsync(string id);

        Task<PageResult<Studant>> GetStudantsAsync(PaginationFilter filter = null);

        Task<bool> ObjectExists(string id);
    }
}
