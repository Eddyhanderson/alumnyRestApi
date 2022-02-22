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
    public class OrganService : IOrganService
    {
        private readonly DataContext dataContext;

        public OrganService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<CreationResult<Organ>> CreateAsync(Organ organ)
        {
            if (organ == null) return FailCreation();

            if (await OrganNameAlreadyExistsAsync(organ.Name))
                return FailCreation();

            var newOrgan = new Organ
            {
                Badget = organ.Badget,
                Code =  await GenerateCodeAsync(),
                Id = Guid.NewGuid().ToString(),
                Name = organ.Name
            };

            try
            {
                await dataContext.Organ.AddAsync(newOrgan);

                var result = await dataContext.SaveChangesAsync();

                return new CreationResult<Organ>
                {
                    Data = newOrgan,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }
        private CreationResult<Organ> FailCreation()
        {
            return new CreationResult<Organ>
            {
                Succeded = false,
                Errors = new string[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private async Task<bool> OrganNameAlreadyExistsAsync(string name)
        {
            return await dataContext.Organ.AnyAsync(o => o.Name.ToUpper() == name);
        }

        private async Task<int> GenerateCodeAsync(){
            var count = await dataContext.Organ.CountAsync();
            return count + 1;
        }

        public async Task<PageResult<Organ>> GetOrgansAsync(PaginationFilter filter = null, OrganQuery query = null)
        {
            var organs = dataContext.Organ.AsQueryable();

            return await GetPaginationAsync(organs, filter);
        }

         private async Task<PageResult<Organ>> GetPaginationAsync(IQueryable<Organ> organ, PaginationFilter filter)
        {
            var totalElement = await organ.CountAsync();

            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                organ = organ
                    .Where(o => o.Name.Contains(sv));
            }

            if (filter.PageNumber >= 0 && filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                organ = organ.Skip(skip).Take(filter.PageSize);
            }

            var page = new PageResult<Organ>
            {
                Data = organ,
                TotalElements = totalElement
            };

            return page;
        }
    }
}
