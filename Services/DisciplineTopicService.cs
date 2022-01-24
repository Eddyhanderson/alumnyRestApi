using alumni.Contracts.V1.Requests.Queries;
using alumni.Data;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Services
{
    public class DisciplineTopicService : IDisciplineTopicService
    {
        private readonly DataContext dataContext;

        private readonly IBadgeInformationService badgeInformationService;

        private readonly IDisciplineService disciplineService;

        public DisciplineTopicService(DataContext dataContext, IBadgeInformationService badgeInformationService, IDisciplineService disciplineService)
        {
            this.dataContext = dataContext;

            this.badgeInformationService = badgeInformationService;

            this.disciplineService = disciplineService;
        }

        public async Task<CreationResult<DisciplineTopic>> CreateAsync(DisciplineTopic disciplineTopic)
        {
            if (disciplineTopic == null)
                return FailCreation();

            var exists = await dataContext.DisciplineTopics
                .AnyAsync(dt => dt.Name.ToUpper() == disciplineTopic.Name.ToUpper());

            if (exists) return FailCreation();

            try
            {
                if (disciplineTopic.BadgeInformationId == null)
                {
                    var stt = await badgeInformationService.CreateAsync(disciplineTopic.BadgeInformation);

                    if (!stt.Succeded) return FailCreation();

                    disciplineTopic.BadgeInformationId = stt.Data.Id;
                }

                return await PersistOnDataBase(disciplineTopic);
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        private async Task<CreationResult<DisciplineTopic>> PersistOnDataBase(DisciplineTopic disciplineTopic)
        {
            try
            {
                var newDisciplineTopic = new DisciplineTopic
                {
                    Id = Guid.NewGuid().ToString(),
                    BadgeInformationId = disciplineTopic.BadgeInformationId,                    
                    Name = disciplineTopic.Name,
                };

                await dataContext.DisciplineTopics.AddAsync(newDisciplineTopic);

                await dataContext.SaveChangesAsync();

                return new CreationResult<DisciplineTopic>
                {
                    Data = newDisciplineTopic,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        private CreationResult<DisciplineTopic> FailCreation()
        {
            return new CreationResult<DisciplineTopic>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        public async Task<DisciplineTopic> GetDisciplineTopicAsync(string disciplineTopicId)
        {
            if (disciplineTopicId == null) return null;

            var disciplineTopic = await dataContext.DisciplineTopics.Include(dt => dt.BadgeInformation)
                .SingleOrDefaultAsync(dt => dt.Id == disciplineTopicId
                && dt.BadgeInformation.Situation == Constants.SituationsObjects.NormalSituation);

            return disciplineTopic;
        }

        public async Task<IEnumerable<DisciplineTopic>> GetDisciplineTopicsAsync(PaginationFilter filter = null, DisciplineTopicQuery disciplineTopicQuery = null)
        {            

            var disciplineTopics = dataContext.DisciplineTopics
                .Include(dt => dt.BadgeInformation)
                .Where(dt => dt.BadgeInformation.Situation == Constants.SituationsObjects.NormalSituation)
                .AsQueryable();
            /*
            if (disciplineTopicQuery != null && disciplineTopicQuery.SchoolId != null && disciplineTopicQuery.TeacherId != null)
                disciplineTopics = (from dt in disciplineTopics
                                   from tp in dataContext.TeacherPlaces
                                   from l in dataContext.Lessons
                                   where dt.Id == l.DiscpilineTopicId && l.TeacherPlaceId == tp.Id && tp.TeacherId == disciplineTopicQuery.TeacherId
                                   && tp.SchoolId == disciplineTopicQuery.SchoolId
                                   select dt).Distinct().OrderBy(dt => dt.Id);

            if (disciplineTopicQuery != null && disciplineTopicQuery.TeacherId != null && disciplineTopicQuery.SchoolId == null)
                disciplineTopics = (from dt in disciplineTopics
                                   from tp in dataContext.TeacherPlaces
                                   from l in dataContext.Lessons
                                   where dt.Id == l.DiscpilineTopicId && l.TeacherPlaceId ==tp.Id && tp.TeacherId == disciplineTopicQuery.TeacherId
                                   select dt).Distinct().OrderBy(dt => dt.Id); ;

            if (disciplineTopicQuery != null && disciplineTopicQuery.TeacherPlaceId != null)
                disciplineTopics = (from dt in disciplineTopics
                                   from l in dataContext.Lessons
                                   where dt.Id == l.DiscpilineTopicId && l.TeacherPlaceId == disciplineTopicQuery.TeacherPlaceId
                                   select dt).Distinct().OrderBy(dt => dt.Id); ;*/

            if (filter == null) return await disciplineTopics.ToListAsync();

            return await GetPaginationAsync(disciplineTopics, filter);
        }

        public Task<bool> ObjectExists(string id)
        {
            return dataContext.DisciplineTopics.AnyAsync(dt => dt.Id == id);
        }

        private async Task<IEnumerable<DisciplineTopic>> GetPaginationAsync(IQueryable<DisciplineTopic> disciplineTopics, PaginationFilter filter)
        {
            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                disciplineTopics = disciplineTopics                    
                    .Where(dt => dt.Name.Contains(sv));
            }

            if (filter.PageNumber >= 0 || filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                disciplineTopics = disciplineTopics.Skip(skip).Take(filter.PageSize);
            }

            return await disciplineTopics.ToListAsync();
        }
    }
}
