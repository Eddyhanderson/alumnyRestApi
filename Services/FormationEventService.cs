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
    public class FormationEventService : IFormationEventService
    {
        private readonly DataContext dataContext;

        public FormationEventService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<CreationResult<FormationEvent>> CreateAsync(FormationEvent formationEvent)
        {
            if (formationEvent == null) return FailCreation();

            var hasFormation = await CheckIfHasFormationsInActiveState(formationEvent.FormationId);

            if (hasFormation) return FailCreation();

            var newFormationEvent = new FormationEvent
            {
                Id = Guid.NewGuid().ToString(),
                Start = formationEvent.Start,
                End = formationEvent.End,
                FormationId = formationEvent.FormationId,
                StudantLimit = formationEvent.StudantLimit,
                Situation = Constants.SituationsObjects.NormalSituation,
                State = FormationEventStates.Waiting.ToString()
            };

            try
            {
                await dataContext.FormationEvents.AddAsync(newFormationEvent);

                var result = await dataContext.SaveChangesAsync();
                
                var result2 = await SetFormationPublished(newFormationEvent.FormationId);
                
                if ( result2 is null)
                    return FailCreation();

                return new CreationResult<FormationEvent>
                {
                    Data = newFormationEvent,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<PageResult<FormationEvent>> GetFormationEventAsync(PaginationFilter filter = null,
            FormationEventQuery query = null)
        {

            var formationEvents = dataContext.FormationEvents.Include(fe => fe.Formation).AsQueryable();

            if (query?.FormationId != null)
                formationEvents = formationEvents.Where(fe => fe.FormationId == query.FormationId);

            if (query?.Situation != null)
                formationEvents = formationEvents.Where(fe => fe.Situation.ToUpper() == query.Situation.ToUpper());

            if (query?.Category != null)
                formationEvents = formationEvents.Where(fe => fe.Formation.Category == query.Category);

            return await GetPaginationAsync(formationEvents, filter);
        }

        public async Task<Formation> GetFormationAsync(string id)
        {
            if (id == null) return null;

            var formation = await dataContext.Formations
                .SingleOrDefaultAsync(f => f.Id == id);

            return formation;
        }

        private CreationResult<FormationEvent> FailCreation()
        {
            return new CreationResult<FormationEvent>
            {
                Succeded = false,
                Errors = new string[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private async Task<PageResult<FormationEvent>> GetPaginationAsync(IQueryable<FormationEvent> formationEvents, PaginationFilter filter)
        {
            var totalElement = await formationEvents.CountAsync();

            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                formationEvents = formationEvents
                    .Where(fe => fe.Formation.Theme.Contains(sv));
            }

            if (filter.PageNumber >= 0 && filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                formationEvents = formationEvents.Skip(skip).Take(filter.PageSize);
            }

            var page = new PageResult<FormationEvent>
            {
                Data = formationEvents,
                TotalElements = totalElement
            };

            return page;
        }

        private async Task<bool> CheckIfHasFormationsInActiveState(string formationId)
        {
            return await dataContext.FormationEvents
            .Where(fe => fe.FormationId == formationId && fe.Situation.ToUpper()
            == Constants.SituationsObjects.NormalSituation.ToUpper()).AnyAsync();
        }

        private async Task<bool?> SetFormationPublished(string formationId)
        {
            var formation = await dataContext.Formations.SingleOrDefaultAsync(f => f.Id == formationId);

            if (formation is null) return null;

            formation.Published = true;
            try
            {
                dataContext.Entry<Formation>(formation).State = EntityState.Modified;
            }
            catch (DbUpdateException e)
            {
                return null;
            }
            var result = await dataContext.SaveChangesAsync();

            return result > 1;

        }
    }
}
