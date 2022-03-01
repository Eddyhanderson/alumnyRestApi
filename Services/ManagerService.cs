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

        private readonly IUserService userService;

        public ManagerService(DataContext dataContext, IUserService userService)
        {
            this.dataContext = dataContext;
            this.userService = userService;
        }

        public async Task<AuthResult> CreateAsync(Manager manager, User user, AuthData auth)
        {
            if (manager == null) return AuthFail();            

            var authResult = await this.userService.RegistrationAsync(user, auth);

            if(authResult is null || !authResult.Authenticated)
                return AuthFail();

            var newManager = new Manager
            {
                Id = Guid.NewGuid().ToString(),
                UserId = authResult.User.Id,
                FirstName = manager.FirstName,
                LastName = manager.LastName
            };

            try
            {
                await dataContext.Managers.AddAsync(newManager);

                var result = await dataContext.SaveChangesAsync();

                return authResult;
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return AuthFail();
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

            var manager = await dataContext.Managers                
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
    
        private AuthResult AuthFail()
        {
            return new AuthResult { Errors = new[] { Constants.ServerMessages.TokenNotCreated } };
        }
    }
}
