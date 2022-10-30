using alumni.Contracts.V1.Requests.Queries;
using alumni.Data;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly DataContext dataContext;

        public SubscriptionService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<CreationResult<Subscription>> CreateAsync(Subscription subscription)
        {
            if (subscription == null) return FailCreation();

            var newSubscription = new Subscription
            {
                Id = Guid.NewGuid().ToString(),
                FormationEventId = subscription.FormationEventId,
                StudantId = subscription.StudantId,
                State = SubscriptionStates.Learning.ToString(),
                Situation = Constants.SituationsObjects.NormalSituation
            };

            try
            {
                await dataContext.Subscriptions.AddAsync(newSubscription);

                var result = await dataContext.SaveChangesAsync();

                if (result > 0)
                    return new CreationResult<Subscription>
                    {
                        Data = newSubscription,
                        Succeded = true
                    };

                return FailCreation();

            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<Subscription> GetAsync(string studantId, string formationId)
        {
            var subscription = await dataContext.Subscriptions
            .Include(s => s.FormationEvent)
            .SingleOrDefaultAsync(s => s.FormationEvent.FormationId == formationId && studantId == s.StudantId
            && s.Situation == Constants.SituationsObjects.NormalSituation);

            return subscription;
        }


        public async Task<PageResult<Subscription>> GetSubscriptionsAsync(PaginationFilter filter, SubscriptionQuery query)
        {
            var subscription = dataContext.Subscriptions
            .Include(s => s.FormationEvent)
                .ThenInclude(fe => fe.Formation)
                    .ThenInclude(f => f.School)
                        .ThenInclude(s => s.User)
            .Include(s => s.Studant).ThenInclude(s => s.User)
            .Where(s => s.Situation == Constants.SituationsObjects.NormalSituation);

            if (query?.State != null)
                subscription = subscription.Where(s => query.State.Contains(s.State));

            if (query?.StudantId != null)
                subscription = subscription.Where(s => s.StudantId == query.StudantId);
            else if (query?.SchoolId != null)
                subscription = subscription.Where(s => s.FormationEvent.Formation.SchoolId == query.SchoolId);

            return await GetPaginationAsync(subscription, filter);
        }

        private CreationResult<Subscription> FailCreation()
        {
            return new CreationResult<Subscription>
            {
                Succeded = false,
                Errors = new string[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private async Task<PageResult<Subscription>> GetPaginationAsync(IQueryable<Subscription> subscription, PaginationFilter filter)
        {
            var totalElement = await subscription.CountAsync();

            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                subscription = subscription
                    .Where(s => s.Studant.FirstName.Contains(sv) || s.Studant.LastName.Contains(sv));
            }

            if (filter.PageNumber >= 0 && filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                subscription = subscription.Skip(skip).Take(filter.PageSize);
            }

            var page = new PageResult<Subscription>
            {
                Data = subscription,
                TotalElements = totalElement
            };

            return page;
        }



    }
}
