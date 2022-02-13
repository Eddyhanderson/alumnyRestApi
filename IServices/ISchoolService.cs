using System.Threading.Tasks;
using alumni.Domain;

public interface ISchoolService
{
    Task<CreationResult<School>> CreateAsync(School identity, User user, AuthData auth);

    Task<School> GetAsync(string id);

    Task<School> GetByUserAsync(string userId);
}