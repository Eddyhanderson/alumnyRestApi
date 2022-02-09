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
    public class TeacherSchoolsService : ITeacherSchoolsService
    {
        private readonly DataContext dataContext;

        public TeacherSchoolsService(DataContext dataContext,
            ITeacherService teacherService,
            ISchoolService schoolService)
        {
            this.dataContext = dataContext;
        }

        public async Task<CreationResult<TeacherSchools>> CreateAsync(TeacherSchools teacherSchools)
        {
            if (teacherSchools == null) return FailCreation();

            var exists = await (dataContext.TeacherSchools
                .SingleOrDefaultAsync(s => s.TeacherId == teacherSchools.TeacherId
                        && s.SchoolId == teacherSchools.SchoolId
                        && s.Situation == Constants.SituationsObjects.PendingSituation)) != null;

            if (exists) return FailCreation();

            try
            {
                var newTeacherSchools = new TeacherSchools
                {
                    SchoolId = teacherSchools.SchoolId,
                    TeacherId = teacherSchools.TeacherId,
                    Situation = Constants.SituationsObjects.PendingSituation,
                    CreationAt = DateTime.UtcNow,
                    DateSituation = DateTime.UtcNow
                };

                await dataContext.TeacherSchools.AddAsync(newTeacherSchools);

                await dataContext.SaveChangesAsync();

                return new CreationResult<TeacherSchools>
                {
                    Data = newTeacherSchools,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<bool> UpdateAsync(string teacherId, string schoolId, TeacherSchools teacherSchools)
        {
            if (teacherId != teacherSchools.TeacherId || schoolId != teacherSchools.SchoolId)
                return false;

            var CreationData = await dataContext.TeacherSchools
                .Where(ts => ts.TeacherId == teacherId && ts.SchoolId == schoolId)
                .Select(ts => ts.CreationAt).FirstOrDefaultAsync();

            var newTeacherSchools = new TeacherSchools
            {
                CreationAt = CreationData,
                DateSituation = DateTime.UtcNow,
                SchoolId = schoolId,
                TeacherId = teacherId,
                Situation = teacherSchools.Situation,
            };

            dataContext.Entry(newTeacherSchools).State = EntityState.Modified;

            try
            {
                var update = await dataContext.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }

        public async Task<IEnumerable<TeacherSchools>> GetAllAsync(PaginationFilter filter, TeacherSchoolQuery query)
        {
            if (query.SchoolId != null && query.TeacherId != null) return null;

            var teacherSchools = dataContext.TeacherSchools.AsQueryable();
             
            if (query.Situation != null)
                teacherSchools = dataContext.TeacherSchools
                    .Where(ts => ts.Situation.ToUpper() == query.Situation.ToUpper());

            if (query.TeacherId != null)
                teacherSchools = teacherSchools
                    /*.Include(ts => ts.School).ThenInclude(s => s.BadgeInformation)*/
                    .Where(ts => ts.TeacherId == query.TeacherId);
            else if (query.SchoolId != null)
                teacherSchools = teacherSchools
                    .Include(ts => ts.Teacher).ThenInclude(t => t.User)
                    .Include(ts => ts.Teacher.Course)
                    .Include(ts => ts.Teacher.Academy)
                    .Include(ts => ts.Teacher.AcademicLevel)
                    .Where(ts => ts.SchoolId == query.SchoolId);

            return await GetPaginationAsync(teacherSchools, filter);

        }

        private async Task<IEnumerable<TeacherSchools>> GetPaginationAsync(IQueryable<TeacherSchools> teacherSchool, PaginationFilter filter)
        {
            if (filter == null) return await teacherSchool.ToListAsync();

            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                /*teacherSchool = teacherSchool
                    .Where(tp => tp.Teacher.User.FirstName.Contains(sv) ||
                    tp.Teacher.User.LastName.Contains(sv) ||
                    tp.School.Name.Contains(sv));*/
            }

            if (filter.PageNumber >= 0 || filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                teacherSchool = teacherSchool.Skip(skip).Take(filter.PageSize);
            }

            return await teacherSchool.ToListAsync();
        }

        public async Task<bool> CheckTeacherHasSchoolAsync(string teacherId)
        {

            var checkResult = await dataContext.TeacherSchools
                .AnyAsync(ts => ts.TeacherId == teacherId && ts.Situation.ToUpper() == Constants.SituationsObjects.NormalSituation.ToUpper());

            return checkResult;
        }

        private CreationResult<TeacherSchools> FailCreation()
        {
            return new CreationResult<TeacherSchools>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private async Task<bool> TeacherSchoolsExists(TeacherSchools teacherSchools)
        {
            return await dataContext.TeacherSchools
                .AnyAsync(ts => ts.TeacherId == teacherSchools.TeacherId && ts.SchoolId == teacherSchools.SchoolId
                && ts.Situation == teacherSchools.Situation);
        }
    }
}
