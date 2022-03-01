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
    public class FormationRequestService : IFormationRequestService
    {
        private readonly DataContext dataContext;

        private readonly ISubscriptionService subscriptionService;

        public FormationRequestService(DataContext dataContext, ISubscriptionService subscriptionService)
        {
            this.dataContext = dataContext;
            this.subscriptionService = subscriptionService;
        }

        public async Task<CreationResult<FormationRequest>> CreateAsync(FormationRequest request)
        {
            if (request == null) return FailCreation();

            var newRequest = new FormationRequest
            {
                Id = Guid.NewGuid().ToString(),
                CreateAt = DateTime.Now,
                FormationId = request.FormationId,
                StudantId = request.StudantId,
                StudantMessage = request.StudantMessage,
                TeacherMessage = request.TeacherMessage,
                State = FormationRequestStates.WatingResponsableAction.ToString("g"),
                Situation = Constants.SituationsObjects.NormalSituation,
                StateDate = DateTime.Now
            };

            try
            {
                await dataContext.FormationRequests.AddAsync(newRequest);

                var result = await dataContext.SaveChangesAsync();

                return new CreationResult<FormationRequest>
                {
                    Data = newRequest,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<FormationRequest> GetFormationRequestAsync(string studantId, string formationId)
        {
            if (studantId is null || formationId is null) return null;

            var query = dataContext.FormationRequests
            .Include(fr => fr.Formation).ThenInclude(f => f.FormationEvents)
            .Include(fr => fr.Formation.School).ThenInclude(s => s.User)
            .Include(fr => fr.Studant).ThenInclude(s => s.User)
            .Where(fr => fr.StudantId == studantId && fr.FormationId == formationId && fr.Situation == Constants.SituationsObjects.NormalSituation);

            var request = await query.FirstOrDefaultAsync();

            if (request is null) return request;
            request.Formation.FormationEvents = request.Formation.FormationEvents.Where(fe => fe.Situation == Constants.SituationsObjects.NormalSituation).ToList();

            return request;
        }

        public async Task<FormationRequest> GetFormationRequestAsync(string id)
        {
            if (id is null) return null;

            var query = dataContext.FormationRequests
            .Include(fr => fr.Formation).ThenInclude(f => f.FormationEvents)
            .Include(fr => fr.Formation.School).ThenInclude(s => s.User)
            .Include(fr => fr.Studant).ThenInclude(s => s.User)
            .Where(fr => fr.Id == id && fr.Situation == Constants.SituationsObjects.NormalSituation);

            var request = await query.FirstOrDefaultAsync();

            if (request is null) return request;
            request.Formation.FormationEvents = request.Formation.FormationEvents.Where(fe => fe.Situation == Constants.SituationsObjects.NormalSituation).ToList();

            return request;
        }

        public async Task<PageResult<FormationRequest>> GetFormationRequestsAsync(PaginationFilter filter = null, FormationRequestQuery query = null)
        {
            var queryResult = dataContext.FormationRequests
            .Include(fr => fr.Formation).ThenInclude(f => f.FormationEvents)
            .Include(fr => fr.Formation.School).ThenInclude(s => s.User)
            .Include(fr => fr.Studant)
            .ThenInclude(s => s.User)
            .Include(f => f.Studant.Organ).AsQueryable();

            if (query.IsResponsable)
            {
                if (query.StudantId is null) return null;
                var studant = await dataContext.Studants.Where(s => s.Id == query.StudantId).SingleOrDefaultAsync();
                queryResult = queryResult.Where(fr => fr.Studant.OrganId == studant.OrganId && fr.Situation == Constants.SituationsObjects.NormalSituation);
            }
            else if (query.StudantId != null)
            {
                queryResult = queryResult.Where(fr => fr.StudantId == query.StudantId && fr.Situation == Constants.SituationsObjects.NormalSituation);
            }
            else if (query.IsManager)
            {
                queryResult = queryResult.Where(fr => fr.Situation == Constants.SituationsObjects.NormalSituation
                && (fr.State == FormationRequestStates.Aproved.ToString() || fr.State == FormationRequestStates.Payed.ToString()));
            } else if (query.IsSchool)
            {
                queryResult = queryResult.Where(fr => fr.Situation == Constants.SituationsObjects.NormalSituation
                && fr.State == FormationRequestStates.Payed.ToString());
            }

            return await GetPaginationAsync(queryResult, filter);
        }

        public async Task<FormationRequest> AproveFormationRequestAsync(string id, FormationRequest requestData)
        {
            if (id is null || requestData.Id != id) return null;

            var request = await dataContext.FormationRequests.SingleOrDefaultAsync(fr => fr.Id == id);

            try
            {
                if (request != null)
                {
                    request.State = FormationRequestStates.Aproved.ToString();
                    dataContext.Entry(request).State = EntityState.Modified;
                    await dataContext.SaveChangesAsync();

                    return request;
                }
                else
                    return null;
            }
            catch (DbException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<FormationRequest> PayFormationRequestAsync(string id, FormationRequest requestData)
        {
            if (id is null || requestData.Id != id) return null;

            var request = await dataContext.FormationRequests
            .Include(fr => fr.Formation)
            .Include(fr => fr.Studant)
            .SingleOrDefaultAsync(fr => fr.Id == id);

            var organ = await dataContext.Organ.SingleOrDefaultAsync(o => o.Id == request.Studant.OrganId);

            if (request is null || organ is null) return null;

            try
            {

                request.State = FormationRequestStates.Payed.ToString();
                dataContext.Entry(request).State = EntityState.Modified;
                var succeeded = await dataContext.SaveChangesAsync() >= 1;
                
                if(succeeded)
                {
                    organ.Badget -= request.Formation.Price;
                    dataContext.Entry<Organ>(organ).State = EntityState.Modified;
                    var succeededOrgan = await dataContext.SaveChangesAsync() >= 1;

                    if(!succeededOrgan)
                        return null;
                }

                return request;

            }
            catch (DbException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<FormationRequest> ConfirmFormationRequestAsync(string id, FormationRequest requestData)
        {
            if (id is null || requestData.Id != id) return null;

            var request = await dataContext.FormationRequests.SingleOrDefaultAsync(fr => fr.Id == id);

            try
            {
                if (request != null)
                {
                    var formationEvent = await dataContext.FormationEvents
                    .SingleOrDefaultAsync(fe => fe.FormationId == request.FormationId
                    && fe.Situation == Constants.SituationsObjects.NormalSituation);

                    if (formationEvent is null) return null;

                    request.State = FormationRequestStates.Confirmed.ToString();
                    request.Situation = Constants.SituationsObjects.History;
                    dataContext.Entry(request).State = EntityState.Modified;

                    var succeeded = await dataContext.SaveChangesAsync() >= 1;

                    if (!succeeded)
                        return null;

                    var subscription = new Subscription
                    {
                        FormationEventId = formationEvent.Id,
                        StudantId = request.StudantId
                    };

                    var crtResult = await subscriptionService.CreateAsync(subscription);

                    if (!crtResult.Succeded) return null;

                    return request;
                }
                else
                    return null;
            }
            catch (DbException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private async Task<PageResult<FormationRequest>> GetPaginationAsync(IQueryable<FormationRequest> request, PaginationFilter filter)
        {
            var totalElement = await request.CountAsync();

            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                request = request
                    .Where(fr => fr.Studant.FirstName.Contains(sv)
                    || fr.Studant.LastName.Contains(sv)
                    || fr.Formation.Theme.Contains(sv)
                    || fr.Formation.Description.Contains(sv));
            }

            if (filter.PageNumber >= 0 && filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                request = request.Skip(skip).Take(filter.PageSize);
            }

            var page = new PageResult<FormationRequest>
            {
                Data = request,
                TotalElements = totalElement
            };

            return page;
        }

        private CreationResult<FormationRequest> FailCreation()
        {
            return new CreationResult<FormationRequest>
            {
                Succeded = false,
                Errors = new string[] { Constants.ModelMessages.FailModelCreated }
            };
        }


    }
}
