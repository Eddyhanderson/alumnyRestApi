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
    public class FormationService : IFormationService
    {
        private readonly DataContext dataContext;

        public FormationService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<CreationResult<Formation>> CreateAsync(Formation formation)
        {
            if (formation == null) return FailCreation();

            var newFormation = new Formation
            {
                Id = Guid.NewGuid().ToString(),
                Category = formation.Category,
                Description = formation.Description,
                Price = formation.Price,
                SchoolId = formation.SchoolId,
                Theme = formation.Theme,
                Situation = Constants.SituationsObjects.NormalSituation,
                DateSituation = DateTime.Now,
                DateCreation = DateTime.Now
            };

            try
            {
                await dataContext.Formations.AddAsync(newFormation);

                var result = await dataContext.SaveChangesAsync();

                return new CreationResult<Formation>
                {
                    Data = newFormation,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<PageResult<Formation>> GetFormationsAsync(PaginationFilter filter = null,
            FormationQuery query = null)
        {

            var formations = dataContext.Formations.Include(f => f.School).ThenInclude(s => s.User)
            .Include(f => f.Modules).ThenInclude(m => m.Lessons)
            .Include(f => f.FormationEvents).ThenInclude(fe => fe.Subscriptions).AsQueryable();

            if (query?.SchoolId != null)
                formations = formations.Where(f => f.SchoolId == query.SchoolId);

            return await GetPaginationAsync(formations, filter);
        }

        public async Task<Formation> GetFormationAsync(string id)
        {
            if (id == null) return null;

            var formation = await dataContext.Formations.Include(f => f.School).ThenInclude(s => s.User)
            .Include(f => f.Modules).ThenInclude(m => m.Lessons)
            .Include(f => f.FormationEvents).ThenInclude(fe => fe.Subscriptions)
            .SingleOrDefaultAsync(f => f.Id == id);

            formation.FormationEvents = formation.FormationEvents.Where(f => f.State != FormationEventStates.Closed.ToString("g")
            && f.Situation == Constants.SituationsObjects.NormalSituation).ToList();

            return formation;
        }

        public async Task<PageResult<Formation>> GetPublishedFormationsAsync(PaginationFilter filter = null, FormationQuery query = null)
        {

            var formations = from fe in dataContext.FormationEvents
            .Include(fe => fe.Subscriptions)
            .Include(fe => fe.Formation).ThenInclude(f => f.School).ThenInclude(s => s.User)
            .Include(fe => fe.Formation.Modules).ThenInclude(m => m.Lessons)
                             where fe.State == FormationEventStates.Waiting.ToString("g")
                             && fe.Situation == Constants.SituationsObjects.NormalSituation
                             select new Formation
                             {
                                 Category = fe.Formation.Category,
                                 DateCreation = fe.Formation.DateCreation,
                                 DateSituation = fe.Formation.DateSituation,
                                 Description = fe.Formation.Description,
                                 Situation = fe.Formation.Situation,
                                 Id = fe.Formation.Id,
                                 Modules = fe.Formation.Modules,
                                 FormationEvents = new List<FormationEvent> { fe },
                                 Picture = fe.Formation.Picture,
                                 Price = fe.Formation.Price,
                                 Published = fe.Formation.Published,
                                 School = fe.Formation.School,
                                 Theme = fe.Formation.Theme
                             };
            
            await formations.ToListAsync();

            return await GetPaginationAsync(formations, filter);
        }

        public async Task<PageResult<Formation>> GetSubscribedFormationsAsync(PaginationFilter filter = null, FormationQuery query = null)
        {
            if (query.StudantId is null) return null;

            var formations = from s in dataContext.Subscriptions
             .Include(s => s.FormationEvent)
             .Include(s => s.FormationEvent.Formation)
             .Include(s => s.FormationEvent.Subscriptions)
             .Include(s => s.FormationEvent.Formation.School).ThenInclude(s => s.User)
             .Include(s => s.FormationEvent.Formation.Modules).ThenInclude(m => m.Lessons)
                             where s.StudantId == query.StudantId && s.Situation == Constants.SituationsObjects.NormalSituation
                                 && s.FormationEvent.Situation == Constants.SituationsObjects.NormalSituation
                             select new Formation
                             {
                                 Category = s.FormationEvent.Formation.Category,
                                 DateCreation = s.FormationEvent.Formation.DateCreation,
                                 DateSituation = s.FormationEvent.Formation.DateSituation,
                                 Description = s.FormationEvent.Formation.Description,
                                 Situation = s.FormationEvent.Formation.Situation,
                                 Id = s.FormationEvent.Formation.Id,
                                 Modules = s.FormationEvent.Formation.Modules,
                                 FormationEvents = new List<FormationEvent> { s.FormationEvent },
                                 Picture = s.FormationEvent.Formation.Picture,
                                 Price = s.FormationEvent.Formation.Price,
                                 Published = s.FormationEvent.Formation.Published,
                                 School = s.FormationEvent.Formation.School,
                                 Theme = s.FormationEvent.Formation.Theme
                             };

            await formations.ToListAsync();

            return await GetPaginationAsync(formations, filter);
        }

        public async Task<PageResult<Formation>> GetFinishedFormationsAsync(PaginationFilter filter = null, FormationQuery query = null)
        {
            if (query.SchoolId is null) return null;

            var formations =

            from fe in dataContext.FormationEvents
            .Include(fe => fe.Formation)
            .ThenInclude(m => m.Modules)
            .ThenInclude(m => m.Lessons)
            .Include(f => f.Formation.School).ThenInclude(s => s.User)
            .Where(fek => fek.Formation.SchoolId == query.SchoolId
            && fek.Situation == Constants.SituationsObjects.NormalSituation
            && fek.State == FormationEventStates.Finished.ToString())
            select new Formation
            {
                Category = fe.Formation.Category,
                DateCreation = fe.Formation.DateCreation,
                DateSituation = fe.Formation.DateSituation,
                Description = fe.Formation.Description,
                Situation = fe.Formation.Situation,
                Id = fe.Formation.Id,
                Modules = fe.Formation.Modules,
                FormationEvents = new List<FormationEvent> { fe },
                Picture = fe.Formation.Picture,
                Price = fe.Formation.Price,
                Published = fe.Formation.Published,
                School = fe.Formation.School,
                Theme = fe.Formation.Theme
            };


            await formations
            .ForEachAsync(fe =>
            {
                fe.FormationEvents.ForEach(fe => {
                    fe.Subscriptions = dataContext.Subscriptions
                    .Where(s => s.FormationEventId == fe.Id 
                    && s.Situation == Constants.SituationsObjects.NormalSituation
                    && s.State == SubscriptionStates.Assessment.ToString()).ToList();
                }); 
            });

            return await GetPaginationAsync(formations, filter);
        }

        private CreationResult<Formation> FailCreation()
        {
            return new CreationResult<Formation>
            {
                Succeded = false,
                Errors = new string[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private async Task<PageResult<Formation>> GetPaginationAsync(IQueryable<Formation> formations, PaginationFilter filter)
        {
            var totalElement = await formations.CountAsync();

            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                formations = formations
                    .Where(f => f.Theme.Contains(sv));
            }

            if (filter.PageNumber >= 0 && filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                formations = formations.Skip(skip).Take(filter.PageSize);
            }

            var page = new PageResult<Formation>
            {
                Data = formations,
                TotalElements = totalElement
            };

            return page;
        }

    }
}
