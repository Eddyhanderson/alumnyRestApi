using alumni.Contracts.V1.Requests.Queries;
using alumni.Data;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace alumni.Services
{
    public class SchoolServiceDeprecated : ISchoolDeprecatedService
    {
        private readonly DataContext dataContext;

        private readonly IBadgeInformationService badgeInformationService;

        public SchoolServiceDeprecated(DataContext dataContext, IBadgeInformationService badgeInformationService)
        {
            this.dataContext = dataContext;

            this.badgeInformationService = badgeInformationService;
        }

        public async Task<CreationResult<School>> CreateAsync(School school)
        {
            if (school == null) return FailCreation();

            /*var exists = await dataContext.Schools
                .AnyAsync(s => s.Nif == school.Nif);*/

            /*if (exists) return FailCreation();*/

            try
            {
                /*if (school.BadgeInformationId == null)
                {

                    var stt = await badgeInformationService.CreateAsync(school.BadgeInformation);

                    if (!stt.Succeded) return FailCreation();
                    school.BadgeInformationId = stt.Data.Id;
                }


                var newSchool = new School
                {
                    Id = Guid.NewGuid().ToString(),
                    Address = school.Address,
                    BadgeInformationId = school.BadgeInformationId,
                    Name = school.Name,
                    Nif = school.Nif
                };*/

                /*await dataContext.Schools.AddAsync(newSchool);*/

                await dataContext.SaveChangesAsync();

                return new CreationResult<School>
                {
                    /*Data = newSchool,*/
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<School> GetSchoolAsync(string id)
        {
            if (id == null) return null;

            //return await dataContext                
                //.Schools
                /*.Include(s => s.BadgeInformation)*/
                //.SingleOrDefaultAsync(s => s.Id == id);
                return new School();
        }

        public async Task<IEnumerable<School>> GetSchoolsAsync(PaginationFilter filter = null, SchoolQuery schoolQuery = null)
        {

            /*var schools = dataContext.Schools
                .Include(s => s.BadgeInformation)
                .Where(s => s.BadgeInformation.Situation == Constants.SituationsObjects.NormalSituation);*/

            if(schoolQuery?.TeacherId != null)
            {                
                var teacherSchoolIds = dataContext
                    .TeacherSchools
                    .Where(ts => ts.TeacherId == schoolQuery.TeacherId &&
                    ts.Situation.ToUpper() == Constants.SituationsObjects.NormalSituation.ToUpper())
                    .Select(ts => ts.SchoolId);

                /*schools = schoolQuery.Subscribed ? schools.Where(s => teacherSchoolIds.Contains(s.Id)) 
                    : schools.Where(s => !teacherSchoolIds.Contains(s.Id));*/
            }


            /*if (filter == null) return await schools.ToListAsync();   */         

            if (filter.SearchValue != null)
            {
                var sv = filter.SearchValue;

                /*schools = schools.Where(s => s.Name.Contains(sv));*/
            }

            if (filter.PageNumber >= 0 && filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                /*schools = schools.Skip(skip).Take(filter.PageSize);*/
            }

            /*return await schools.ToListAsync();*/
            return new List<School>();
        }

        private CreationResult<School> FailCreation()
        {
            return new CreationResult<School>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }
    }
}
