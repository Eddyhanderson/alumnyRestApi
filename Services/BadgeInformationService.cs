using alumni.Data;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Services
{
    public class BadgeInformationService : IBadgeInformationService
    {
        private readonly DataContext dataContext;

        private readonly IServiceProvider serviceProvider;

        public BadgeInformationService(DataContext dataContext, IServiceProvider serviceProvider)
        {
            this.dataContext = dataContext;

            this.serviceProvider = serviceProvider;
        }

        public async Task<CreationResult<BadgeInformation>> CreateAsync(BadgeInformation badgeInformation)
        {
            if (badgeInformation == null)
                badgeInformation = new BadgeInformation();

            try
            {
                if(badgeInformation.UserId == null)
                {
                    var accessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

                    var userId = UserHelpers.GetUser(accessor.HttpContext);

                    badgeInformation.UserId = userId;
                }

                var newBadgeInformation = new BadgeInformation
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatAt = DateTime.UtcNow,
                    DateSituation = DateTime.UtcNow,
                    ProfilePhotoPath = badgeInformation.ProfilePhotoPath,
                    UserId = badgeInformation.UserId,
                    Situation = Constants.SituationsObjects.NormalSituation
                };

                await dataContext.BadgeInformations.AddAsync(newBadgeInformation);

                await dataContext.SaveChangesAsync();

                return new CreationResult<BadgeInformation>
                {
                    Data = newBadgeInformation,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<BadgeInformation> GetAsync(string id)
        {
            if (id == null) return null;

            var badgeInformation = await dataContext.BadgeInformations.Include(b => b.User).SingleOrDefaultAsync(b => b.Id == id);

            return badgeInformation;
        }

        private CreationResult<BadgeInformation> FailCreation()
        {
            return new CreationResult<BadgeInformation>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }
    }
}
