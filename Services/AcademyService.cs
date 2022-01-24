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
    public class AcademyService : IAcademyService
    {
        private readonly DataContext dataContext;

        private readonly IBadgeInformationService badgeInformationService;

        public AcademyService(DataContext dataContext, IBadgeInformationService badgeInformationService)
        {
            this.dataContext = dataContext;

            this.badgeInformationService = badgeInformationService;
        }

        public async Task<CreationResult<Academy>> CreationAsync(Academy academy)
        {
            if (academy == null)
                return FailCreation();

            var academyStored = await dataContext.Academies
                .SingleOrDefaultAsync(a => a.Name.ToUpper() == academy.Name.ToUpper());

            if (academyStored != null)
                return new CreationResult<Academy>
                {
                    Exists = true,
                    Succeded = true,
                    Data = academyStored,
                };

            try
            {
                if (academy.BadgeInformationId == null)
                {
                    var stt = await badgeInformationService.CreateAsync(academy.BadgeInformation);

                    if (!stt.Succeded) return FailCreation();

                    academy.BadgeInformationId = stt.Data.Id;
                }

                var newAcademy = new Academy
                {
                    Id = Guid.NewGuid().ToString(),
                    BadgeInformationId = academy.BadgeInformationId,
                    Name = academy.Name
                };

                await dataContext.Academies.AddAsync(newAcademy);

                await dataContext.SaveChangesAsync();

                return new CreationResult<Academy>
                {
                    Data = newAcademy,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<Academy> GetAsync(AcademyQuery query)
        {
            if (query.Id == null && query.Name == null) return null;
            if (query.Id != null && query.Name != null) return null;

            if (query.Id != null)
            {
                return await dataContext
                    .Academies.SingleOrDefaultAsync(c => c.Id == query.Id);
            }

            if (query.Name != null)
            {
                return await dataContext
                    .Academies.SingleOrDefaultAsync(c => c.Name == query.Name);
            }
            return null;
        }

        public async Task<IEnumerable<Academy>> GetAllAsync(PaginationFilter filter)
        {
            var academies = from a in dataContext.Academies.Include(a => a.BadgeInformation)
                          where a.BadgeInformation.Situation == Constants.SituationsObjects.NormalSituation
                          select a;


            if (filter == null) return await academies.ToListAsync();

            if (filter.SearchValue != null)
            {
                var sv = filter.SearchValue;

                academies = academies.Where(a => a.Name.Contains(sv));
            }

            if (filter.PageNumber >= 0 && filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                academies = academies.Skip(skip).Take(filter.PageSize);
            }

            return academies;
        }

        private CreationResult<Academy> FailCreation()
        {
            return new CreationResult<Academy>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }        
    }
}
