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
    public class ModuleService : IModuleService
    {
        private readonly DataContext dataContext;

        public ModuleService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<CreationResult<Module>> CreateAsync(Module module)
        {
            if (module == null) return FailCreation();

            var newModule = new Module
            {
                Id = Guid.NewGuid().ToString(),
                Name = module.Name,
                Description = module.Description,
                FormationId = module.FormationId,
                Situation = Constants.SituationsObjects.NormalSituation,
                DateCreation = DateTime.Now,
                DateSituation= DateTime.Now,
                Picture = module.Picture
            };

            try
            {
                await dataContext.Modules.AddAsync(newModule);

                var result = await dataContext.SaveChangesAsync();

                return new CreationResult<Module>
                {
                    Data = newModule,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }
        private CreationResult<Module> FailCreation()
        {
            return new CreationResult<Module>
            {
                Succeded = false,
                Errors = new string[] { Constants.ModelMessages.FailModelCreated }
            };
        }
    }
}
