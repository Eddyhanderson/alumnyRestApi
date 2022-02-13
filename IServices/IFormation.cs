using System.Threading.Tasks;
using alumni.Domain;

namespace alumni.IServices
{
    public interface IFormationService
    {
        Task<CreationResult<Formation>> CreateAsync(Formation formation);
    }
}