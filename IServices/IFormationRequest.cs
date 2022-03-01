using System.Threading.Tasks;
using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;

namespace alumni.IServices
{
    public interface IFormationRequestService{
        Task<CreationResult<FormationRequest>> CreateAsync(FormationRequest request);

        Task<FormationRequest> GetFormationRequestAsync(string studantId, string formationId);

        Task<FormationRequest> GetFormationRequestAsync(string id);

        Task<FormationRequest> AproveFormationRequestAsync(string id, FormationRequest request);

        Task<FormationRequest> PayFormationRequestAsync(string id, FormationRequest requestData);

        Task<FormationRequest> ConfirmFormationRequestAsync(string id, FormationRequest requestData);
        
        Task<PageResult<FormationRequest>> GetFormationRequestsAsync(PaginationFilter filter = null, FormationRequestQuery query = null);

    }
}