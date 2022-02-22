using System.Threading.Tasks;
using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;

public interface IModuleService
{
    Task<CreationResult<Module>> CreateAsync(Module module);

    Task<PageResult<Module>> GetModulesAsync(PaginationFilter filter = null, ModuleQuery query = null);

    Task<Module> GetModuleAsync(string id);
}