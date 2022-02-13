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
        private CreationResult<Formation> FailCreation()
        {
            return new CreationResult<Formation>
            {
                Succeded = false,
                Errors = new string[] { Constants.ModelMessages.FailModelCreated }
            };
        }
    }
}
