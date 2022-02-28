using System.Threading.Tasks;
using alumni.Domain;

public interface ISubscriptionService
{
    Task<CreationResult<Subscription>> CreateAsync(Subscription subscription);

    Task<Subscription> GetAsync(string studantId, string formationId);

}