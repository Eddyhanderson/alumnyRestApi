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

    }
}
