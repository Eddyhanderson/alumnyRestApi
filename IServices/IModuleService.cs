using System.Threading.Tasks;
using alumni.Domain;

public interface IModuleService{
    Task<CreationResult<Module>> CreateAsync(Module module);
}