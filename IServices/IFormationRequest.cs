using System.Threading.Tasks;
using alumni.Domain;

namespace alumni.IServices
{
    public interface IFormationRequestService{
        Task<CreationResult<FormationRequest>> CreateAsync(FormationRequest request);

        Task<FormationRequest> GetFormationRequestAsync(string studantId, string formationId);
    }
}