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
    public class StudantService : IStudantService
    {
        private readonly DataContext dataContext;

        private readonly IUserService userService;

        public StudantService(DataContext dataContext, IUserService userService)
        {
            this.dataContext = dataContext;

            this.userService = userService;
        }

        public async Task<CreationResult<Studant>> CreateAsync(Studant studant, User user, AuthData auth)
        {
            if (studant == null) return FailCreation();

            var authResult = await this.userService.RegistrationAsync(user, auth);

            if (authResult is null || !authResult.Authenticated)
                return FailCreation();

            var newStudant = new Studant
            {
                Id = Guid.NewGuid().ToString(),
                UserId = authResult.User.Id,
                FirstName = studant.FirstName,
                LastName = studant.LastName,
                StudantCode = studant.StudantCode,
                OrganId = studant.OrganId,
                Responsable = studant.Responsable
            };

            try
            {
                await dataContext.Studants.AddAsync(newStudant);
                await dataContext.SaveChangesAsync();

                return new CreationResult<Studant>
                {
                    Data = newStudant,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<PageResult<Studant>> GetStudantsAsync(PaginationFilter filter = null)
        {
            var studants = dataContext.Studants
                           /*.Include(s => s.AcademicLevel)*/
                           /*.Include(s => s.Academy)*/
                           /*.Include(s => s.Course)*/
                           .Include(s => s.User)
                           .Where(s => s.User.Situation == Constants.SituationsObjects.NormalSituation);

            var totalElement = await studants.CountAsync();

            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                studants = studants.Include(s => s.User);

                /*studants = studants.Where(s => s.User.FirstName.Contains(sv)
                || s.User.LastName.Contains(sv) || s.StudantCode.Contains(sv));*/
            }

            if (filter.PageNumber >= 0 || filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                studants = studants.Skip(skip).Take(filter.PageSize);
            }

            var result = new PageResult<Studant>
            {
                Data = await studants.ToListAsync(),
                TotalElements = totalElement
            };

            return result;
        }

        public async Task<Studant> GetStudantAsync(string id)
        {
            if (id == null) return null;

            var studant = await dataContext.Studants
                    .Include(s => s.User)
                    .SingleOrDefaultAsync(s => s.Id == id);

            return studant;
        }

        public async Task<bool> ObjectExists(string id)
        {
            return await dataContext.Studants.AnyAsync(s => s.Id == id);
        }

        public async Task<Studant> GetStudantResponsableAsync(string id)
        {
            var studant = await dataContext.Studants.SingleOrDefaultAsync(s => s.Id == id);

            if(studant is null) return null;

            var responsable = from s in dataContext.Studants
            .Include(s => s.User)
            .Include(s => s.Organ)
            from o in dataContext.Organ
            where s.Responsable && s.OrganId == studant.OrganId
            select s;

            return await responsable.FirstOrDefaultAsync();
        }
        private async Task<string> GenerateStudantCodeAsync(Studant studant)
        {
            var user = await userService.GetUserAsync(studant.UserId);

            /*var prefix = string.Concat(user.FirstName[0], user.LastName[0]);*/

            var seq = await dataContext.Studants.CountAsync() + 1;

            var year = DateTime.Today.Year;

            /*return string.Concat(prefix, year, seq);*/
            return "";
        }

        private CreationResult<Studant> FailCreation()
        {
            return new CreationResult<Studant>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        
    }
}
