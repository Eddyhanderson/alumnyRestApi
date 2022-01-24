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
    public class DisciplineService : IDisciplineService
    {
        private readonly DataContext dataContext;

        private readonly IBadgeInformationService badgeInformationService;

        public DisciplineService(DataContext dataContext, IBadgeInformationService badgeInformationService)
        {
            this.dataContext = dataContext;

            this.badgeInformationService = badgeInformationService;
        }

        public async Task<CreationResult<Discipline>> CreateAsync(Discipline discipline)
        {
            if (discipline == null)
                return FailCreation();

            var exists = await dataContext.Disciplines
                .AnyAsync(d => d.Name.ToUpper() == discipline.Name.ToUpper());

            if (exists)
                return FailCreation();

            try
            {
                if (discipline.BadgeInformationId == null)
                {
                    var stt = await badgeInformationService.CreateAsync(discipline.BadgeInformation);

                    if (!stt.Succeded) return FailCreation();

                    discipline.BadgeInformationId = stt.Data.Id;
                }


                var newDiscipline = new Discipline
                {
                    Id = Guid.NewGuid().ToString(),
                    BadgeInformationId = discipline.BadgeInformationId,
                    Name = discipline.Name
                };

                await dataContext.Disciplines.AddAsync(newDiscipline);

                await dataContext.SaveChangesAsync();

                return new CreationResult<Discipline>
                {
                    Data = newDiscipline,
                    Succeded = true
                };

            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<Discipline> GetDisciplineAsync(string disciplineId)
        {
            if (disciplineId == null) return null;

            var discipline = await dataContext.Disciplines.Include(d => d.BadgeInformation)
                .SingleOrDefaultAsync(d => d.Id == disciplineId
                && d.BadgeInformation.Situation == Constants.SituationsObjects.NormalSituation);

            return discipline;
        }

        public async Task<IEnumerable<Discipline>> GetDisciplinesAsync(PaginationFilter filter = null)
        {
            var disciplines = dataContext.Disciplines
                .Include(dt => dt.BadgeInformation)
                .Where(dt => dt.BadgeInformation.Situation == Constants.SituationsObjects.NormalSituation)
                .AsQueryable();

            if (filter == null) return await disciplines.ToListAsync();

            return await GetPaginationAsync(disciplines, filter);
        }

        private async Task<IEnumerable<Discipline>> GetPaginationAsync(IQueryable<Discipline> disciplines, PaginationFilter filter)
        {
            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                disciplines = disciplines
                    .Where(d => d.Name.Contains(sv));
            }

            if (filter.PageNumber >= 0 || filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                disciplines = disciplines.Skip(skip).Take(filter.PageSize);
            }

            return await disciplines.ToListAsync();
        }

        private CreationResult<Discipline> FailCreation()
        {
            return new CreationResult<Discipline>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        public Task<bool> ObjectExists(string id)
        {
            return dataContext.Disciplines.AnyAsync(d => d.Id == id);
        }
    }
}
