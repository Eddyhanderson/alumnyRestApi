using System.Threading.Tasks;
using alumni.Domain;

namespace alumni.IServices
{
    public interface ICertificateService
    {
        Task<CreationResult<Certificate>> CreateAsync(Certificate certificate);

        Task<Certificate> GetCertificateBySubscriptionAsync(string subsciptionId);
    }
}