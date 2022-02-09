using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IManagerService
    {
        Task<AuthResult> CreateAsync(Manager manager, User user, AuthData auth);

        Task<Manager> GetAsync(string id);

        Task<Manager> GetByUserAsync(string userId);
    }
}
