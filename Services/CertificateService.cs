using alumni.Contracts.V1.Requests.Queries;
using alumni.Data;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly DataContext dataContext;

        public CertificateService(DataContext dataContext)
        {
            this.dataContext = dataContext;

        }

        public async Task<CreationResult<Certificate>> CreateAsync(Certificate certificate)
        {
            if (certificate == null)
                return FailCreation();

            try
            {

                await dataContext.Database.BeginTransactionAsync();

                var subscription = await dataContext.Subscriptions.SingleOrDefaultAsync(s => s.Id == certificate.SubscriptionId);

                if (subscription is null)
                    return FailCreation();

                subscription.State = SubscriptionStates.Closed.ToString();

                dataContext.Entry(subscription).State = EntityState.Modified;

                var succeeded = await dataContext.SaveChangesAsync() > 0;

                if (!succeeded) return FailCreation();

                var hasMore = await dataContext.Subscriptions.AnyAsync(s => s.State != SubscriptionStates.Closed.ToString()
                && s.Situation == Constants.SituationsObjects.NormalSituation.ToString()
                && s.FormationEventId == subscription.FormationEventId);

                if (!hasMore)
                {
                    var events = await dataContext.FormationEvents
                    .Include(fe => fe.Formation)
                    .SingleOrDefaultAsync(fe => fe.Id == subscription.FormationEventId
                    && fe.Situation == Constants.SituationsObjects.NormalSituation.ToString() && fe.State == FormationEventStates.Finished.ToString());

                    events.State = FormationEventStates.Closed.ToString();
                    dataContext.Entry(events).State = EntityState.Modified;
                    var succeededEvent = await dataContext.SaveChangesAsync() > 0;

                    if (!succeededEvent)
                    {
                        dataContext.Database.RollbackTransaction();
                        return FailCreation();
                    }


                    var formation = events.Formation;

                    formation.Published = false;
                    dataContext.Entry(formation).State = EntityState.Modified;

                    var succeededFormation = await dataContext.SaveChangesAsync() > 0;

                    if (!succeededFormation)
                    {
                        dataContext.Database.RollbackTransaction();
                        return FailCreation();
                    }
                }


                var newCertificate = new Certificate
                {
                    Id = Guid.NewGuid().ToString(),
                    AssessmentMethod = certificate.AssessmentMethod,
                    AssessmentScore = certificate.AssessmentScore,
                    MaxScore = certificate.MaxScore,
                    EmitedAt = DateTime.Now,
                    Observation = certificate.Observation,
                    QualitativeResult = certificate.QualitativeResult,
                    PathCertificate = certificate.PathCertificate,
                    SubscriptionId = certificate.SubscriptionId,
                    AssignmentSchool = certificate.AssignmentSchool
                };

                await dataContext.Certificates.AddAsync(newCertificate);

                var succeededCertificate = await dataContext.SaveChangesAsync() > 0;

                if (succeededCertificate)
                {
                    dataContext.Database.CommitTransaction();
                }
                else
                {
                    dataContext.Database.RollbackTransaction();
                }

                return new CreationResult<Certificate>
                {
                    Data = newCertificate,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);
                dataContext.Database.RollbackTransaction();

                return FailCreation();
            }
        }

        public async Task<Certificate> GetCertificateBySubscriptionAsync(string subsciptionId)
        {
            return await dataContext.Certificates.SingleOrDefaultAsync(c => c.SubscriptionId == subsciptionId);
        }

        private CreationResult<Certificate> FailCreation()
        {
            return new CreationResult<Certificate>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }

    }
}
