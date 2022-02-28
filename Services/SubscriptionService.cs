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

        private CreationResult<Subscription> FailCreation()
        {
            return new CreationResult<Subscription>
            {
                Succeded = false,
                Errors = new string[] { Constants.ModelMessages.FailModelCreated }
            };
        }


    }
}
