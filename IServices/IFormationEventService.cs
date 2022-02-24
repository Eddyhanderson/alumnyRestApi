using System.Threading.Tasks;
using alumni.Domain;

namespace alumni.IServices
{
    public interface IFormationEventService
    {
        Task<CreationResult<FormationEvent>> CreateAsync(FormationEvent formationEvent);
    }
}