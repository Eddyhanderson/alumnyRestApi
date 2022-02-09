using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IUserService
    {
        Task<AuthResult> RegistrationAsync(User user,  AuthData auth);

        Task<AuthResult> LoginAsync(LoginDomain login);

        Task<string> GetRoleAsync(string id);

        Task<AuthResult> CreateTokenAsync(User user);

        Task<AuthResult> RefreshTokenAsync(string token, string refreshToken);

        Task<User> GetUserAsync(string id);

        Task<bool> UpdateAsync(string id, User user);        

        Task<IEnumerable<User>> GetUsersAsync(PaginationFilter filter = null);

        Task<Teacher> GetTeacherAsync(string userId);

        Task<Studant> GetStudantAsync(string userId);
    }
}
