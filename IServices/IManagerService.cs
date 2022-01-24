using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IManagerService
    {
        Task<CreationResult<Manager>> CreateAsync(Manager manager);

        Task<Manager> GetAsync(string id);

        Task<Manager> GetByUserAsync(string userId);
    }
}
