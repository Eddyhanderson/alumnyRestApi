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
    public class ManagerService : IManagerService
    {
        private readonly DataContext dataContext;

        public ManagerService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
                                                                            
        public async Task<CreationResult<Manager>> CreateAsync(Manager manager)
        {
            if (manager == null) return FailCreation();

            var newManager = new Manager
            {
                Id = Guid.NewGuid().ToString(),
                UserId = manager.UserId,
                /*SchoolId = manager.SchoolId,
                Situation = Constants.SituationsObjects.NormalSituation,
                DateSituation = DateTime.UtcNow*/
            };

            try
            {
                await dataContext.Managers.AddAsync(newManager);

                var result = await dataContext.SaveChangesAsync();

                return new CreationResult<Manager>
                {
                    Data = newManager,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }

        }

        public async Task<Manager> GetAsync(string id)
        {
            if (id == null) return null;

            var manager = await dataContext.Managers/*.
                Include(m => m.School).ThenInclude(s => s.BadgeInformation)*/
                .SingleOrDefaultAsync(m => m.Id == id);

            return manager;
        }

        public async Task<Manager> GetByUserAsync(string userId)
        {
            if (userId == null) return null;

            var manager = await dataContext.Managers/*.
                Include(m => m.School).ThenInclude(s => s.BadgeInformation)*/
                .SingleOrDefaultAsync(m => m.UserId == userId);

            return manager;
        }

        private CreationResult<Manager> FailCreation()
        {
            return new CreationResult<Manager>
            {
                Succeded = false,
                Errors = new string[] { Constants.ModelMessages.FailModelCreated }
            };
        }
    }
}
