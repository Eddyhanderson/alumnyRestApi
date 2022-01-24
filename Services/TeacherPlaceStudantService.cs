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
    public class TeacherPlaceStudantService : ITeacherPlaceStudantService
    {
        private readonly DataContext dataContext;

        private readonly IStudantService studantService;

        public TeacherPlaceStudantService(DataContext dataContext, IStudantService studantService)
        {
            this.studantService = studantService;

            this.dataContext = dataContext;
        }

        public async Task<TeacherPlaceStudants> GetAsync(string teacherPlaceId, string studantId)
        {
            return await dataContext.TeacherPlaceStudants
                .Include(tps => tps.Studant).ThenInclude(s => s.User)
                .Include(tps => tps.TeacherPlace)
                .SingleOrDefaultAsync(tps => tps.StudantId == studantId && tps.TeacherPlaceId == teacherPlaceId);
        }

        public async Task<CreationResult<TeacherPlaceStudants>> CreateAsync(TeacherPlaceStudants teacherPlaceStudants)
        {
            if (teacherPlaceStudants == null) return FailCreation();

            var newData = new TeacherPlaceStudants
            {
                DateSituation = DateTime.UtcNow,
                RegisteredAt = DateTime.UtcNow,
                Months = teacherPlaceStudants.Months,
                Situation = TeacherPlaceRegistrationState.Registered.ToString("g"),
                TeacherPlaceId = teacherPlaceStudants.TeacherPlaceId,
                StudantId = teacherPlaceStudants.StudantId
            };

            await dataContext.TeacherPlaceStudants.AddAsync(newData);

            try
            {
                await dataContext.SaveChangesAsync();

                return new CreationResult<TeacherPlaceStudants>
                {
                    Data = teacherPlaceStudants,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<IEnumerable<TeacherPlace>> GetTeacherPlacesByStudantAsync(string studantId, PaginationFilter filter = null)
        {
            var teacherPlace = dataContext.TeacherPlaceStudants.Include(tps => tps.TeacherPlace)
                .Where(tps => tps.StudantId == studantId && tps.Situation == Constants.SituationsObjects.NormalSituation)
                .Select(tps => tps.TeacherPlace).AsQueryable();

            if (filter == null) return await teacherPlace.ToListAsync();

            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                teacherPlace = teacherPlace
                    .Where(tp => tp.Name.Contains(sv) || tp.TeacherPlaceCode.Contains(sv));
            }

            if (filter.PageNumber >= 0 || filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                teacherPlace = teacherPlace.Skip(skip).Take(filter.PageSize);
            }

            return await teacherPlace.ToListAsync();
        }

        public async Task<bool> UpdateAsync(string teacherPlaceId, string studantId, TeacherPlaceStudants teacherPlaceStudant)
        {
            if (teacherPlaceId != teacherPlaceStudant.TeacherPlaceId
                || studantId != teacherPlaceStudant.StudantId)
                return false;

            var updateModel = new TeacherPlaceStudants
            {
                StudantId = teacherPlaceStudant.StudantId,
                TeacherPlaceId = teacherPlaceStudant.TeacherPlaceId,
                Situation = teacherPlaceStudant.Situation,
                DateSituation = DateTime.UtcNow
            };

            dataContext.Entry(updateModel).State = EntityState.Modified;

            try
            {
                await dataContext.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }

        public async Task<IEnumerable<Studant>> GetStudantsByTeacherPlaceAsync(string teacherPlaceId, PaginationFilter filter = null)
        {
            var studants = dataContext.TeacherPlaceStudants.Include(tps => tps.Studant)
                .Where(tps => tps.TeacherPlaceId == teacherPlaceId && tps.Situation == Constants.SituationsObjects.NormalSituation)
                .Select(tps => tps.Studant).AsQueryable();

            if (filter == null) return await studants.ToListAsync();

            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                studants = studants.Include(s => s.User)
                    .Where(s => s.User.FirstName.Contains(sv) || s.User.LastName.Contains(sv));
            }

            if (filter.PageNumber >= 0 || filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                studants = studants.Skip(skip).Take(filter.PageSize);
            }

            return await studants.ToListAsync();
        }

        public async Task<bool> ObjectExists(string teacherPlaceId, string studantId)
        {
            return await dataContext.TeacherPlaceStudants.AnyAsync(tps => tps.StudantId == studantId
            && tps.TeacherPlaceId == teacherPlaceId);
        }

        private CreationResult<TeacherPlaceStudants> FailCreation()
        {
            return new CreationResult<TeacherPlaceStudants>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }
    }
}
