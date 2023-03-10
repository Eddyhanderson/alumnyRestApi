using System.Threading.Tasks;
using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;

public interface ISubscriptionService
{
    Task<CreationResult<Subscription>> CreateAsync(Subscription subscription);

    Task<Subscription> GetAsync(string studantId, string formationId);

    Task<PageResult<Subscription>> GetSubscriptionsAsync(PaginationFilter filter, SubscriptionQuery query);

}