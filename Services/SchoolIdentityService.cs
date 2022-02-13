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
    public class SchoolIdentityService : ISchoolIdentityService
    {
        private readonly DataContext dataContext;

        private readonly IUserService userService;

        public SchoolIdentityService(DataContext dataContext, IUserService userService)
        {
            this.dataContext = dataContext;
            this.userService = userService;
        }

        public async Task<CreationResult<SchoolIdentity>> CreateAsync(SchoolIdentity identity, User user, AuthData auth)
        {
            if (identity == null) return FailCreation();

            var authResult = await this.userService.RegistrationAsync(user, auth);

            if (authResult is null || !authResult.Authenticated)
                return FailCreation();

            var newSchoolIdentity = new SchoolIdentity
            {
                Id = Guid.NewGuid().ToString(),
                UserId = authResult.User.Id,
                Name = identity.Name,
                Acronym = identity.Acronym,
                Adress = identity.Adress,
                Nif = identity.Nif,
                SchoolCode = await GenerateCodeAsync(identity.Name)
            };

            try
            {
                await dataContext.SchoolIdentities.AddAsync(newSchoolIdentity);

                await dataContext.SaveChangesAsync();

                await GenerateItsSchoolAsync(newSchoolIdentity);

                return new CreationResult<SchoolIdentity>
                {
                    Data = newSchoolIdentity,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }

        }

        public async Task<SchoolIdentity> GetAsync(string id)
        {
            if (id == null) return null;

            var schoolIdentity = await dataContext.SchoolIdentities
                  .SingleOrDefaultAsync(si => si.Id == id);

            return schoolIdentity;
        }

        public async Task<SchoolIdentity> GetByUserAsync(string userId)
        {
            if (userId == null) return null;

            var schoolIdentity = await dataContext.SchoolIdentities
                .SingleOrDefaultAsync(si => si.UserId == userId);

            return schoolIdentity;
        }

        private CreationResult<SchoolIdentity> FailCreation()
        {
            return new CreationResult<SchoolIdentity>
            {
                Succeded = false,
                Errors = new string[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private async Task<string> GenerateCodeAsync(string name)
        {
            var count = await dataContext.SchoolIdentities.CountAsync() + 1;
            string prefix = "";
            if (name.Length > 1)
                prefix = name.Substring(0, 1);
            var code = prefix + DateTime.UtcNow.Year + count;
            return code;
        }

        private async Task GenerateItsSchoolAsync(SchoolIdentity identity)
        {
            var school = new School
            {
                Id = Guid.NewGuid().ToString(),
                SchoolIdentityId = identity.Id
            };

            await dataContext.Schools.AddAsync(school);

            await dataContext.SaveChangesAsync();
        }
    }
}
