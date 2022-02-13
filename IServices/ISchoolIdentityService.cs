using System.Threading.Tasks;
using alumni.Domain;

public interface ISchoolIdentityService
{
    Task<CreationResult<SchoolIdentity>> CreateAsync(SchoolIdentity identity, User user, AuthData auth);

    Task<SchoolIdentity> GetAsync(string id);

    Task<SchoolIdentity> GetByUserAsync(string userId);
}