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
    public class SchoolService : ISchoolService
    {
        private readonly DataContext dataContext;

        private readonly IUserService userService;

        public SchoolService(DataContext dataContext, IUserService userService)
        {
            this.dataContext = dataContext;
            this.userService = userService;
        }

        public async Task<CreationResult<School>> CreateAsync(School school, User user, AuthData auth)
        {
            if (school == null) return FailCreation();

            var authResult = await this.userService.RegistrationAsync(user, auth);

            if (authResult is null || !authResult.Authenticated)
                return FailCreation();

            var newSchool = new School
            {
                Id = Guid.NewGuid().ToString(),
                UserId = authResult.User.Id,
                Name = school.Name,
                Acronym = school.Acronym,
                Adress = school.Adress,
                Nif = school.Nif,
                SchoolCode = await GenerateCodeAsync(school.Name)
            };

            try
            {
                await dataContext.Schools.AddAsync(newSchool);
                await dataContext.SaveChangesAsync();

                return new CreationResult<School>
                {
                    Data = newSchool,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }

        }

        public async Task<School> GetAsync(string id)
        {
            if (id == null) return null;

            var school = await dataContext.Schools
                  .SingleOrDefaultAsync(si => si.Id == id);

            return school;
        }

        public async Task<School> GetByUserAsync(string userId)
        {
            if (userId == null) return null;

            var school = await dataContext.Schools
            .Include(s => s.User)
            .SingleOrDefaultAsync(s => s.UserId == userId);

            return school;
        }

        private CreationResult<School> FailCreation()
        {
            return new CreationResult<School>
            {
                Succeded = false,
                Errors = new string[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private async Task<string> GenerateCodeAsync(string name)
        {
            var count = await dataContext.Schools.CountAsync() + 1;
            string prefix = "";
            if (name.Length > 1)
                prefix = name.Substring(0, 1);
            var code = prefix + DateTime.UtcNow.Year + count;
            return code;
        }
    }
}
