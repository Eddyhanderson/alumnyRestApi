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
                DateSituation = DateTime.Now,
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

        public async Task<PageResult<Module>> GetModulesAsync(PaginationFilter filter = null,
            ModuleQuery query = null)
        {

            var modules = from s in dataContext.Modules
                          where s.FormationId == query.FormationId
                          select s;

            return await GetPaginationAsync(modules, filter);
        }

        private CreationResult<Module> FailCreation()
        {
            return new CreationResult<Module>
            {
                Succeded = false,
                Errors = new string[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private async Task<PageResult<Module>> GetPaginationAsync(IQueryable<Module> module, PaginationFilter filter)
        {
            var totalElement = await module.CountAsync();

            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                module = module
                    .Where(m => m.Name.Contains(sv) || m.Description.Contains(sv));
            }

            if (filter.PageNumber >= 0 && filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                module = module.Skip(skip).Take(filter.PageSize);
            }

            var page = new PageResult<Module>
            {
                Data = module,
                TotalElements = totalElement
            };

            return page;
        }
    }
}
