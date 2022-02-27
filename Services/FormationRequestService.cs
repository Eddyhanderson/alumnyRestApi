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
    public class FormationRequestService : IFormationRequestService
    {
        private readonly DataContext dataContext;

        public FormationRequestService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<CreationResult<FormationRequest>> CreateAsync(FormationRequest request)
        {
            if (request == null) return FailCreation();

            var newRequest = new FormationRequest
            {
                Id = Guid.NewGuid().ToString(),
                CreateAt = DateTime.Now,
                FormationId = request.FormationId,
                StudantId = request.StudantId,
                StudantMessage = request.StudantMessage,
                TeacherMessage = request.TeacherMessage,
                State = FormationRequestStates.WatingResponsableAction.ToString("g"),
                Situation = Constants.SituationsObjects.NormalSituation,
                StateDate = DateTime.Now
            };

            try
            {
                await dataContext.FormationRequests.AddAsync(newRequest);

                var result = await dataContext.SaveChangesAsync();

                return new CreationResult<FormationRequest>
                {
                    Data = newRequest,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<FormationRequest> GetFormationRequestAsync(string studantId, string formationId)
        {
            if (studantId is null || formationId is null) return null;

            var query = dataContext.FormationRequests
            .Include(fr => fr.Formation).ThenInclude(f => f.FormationEvents)
            .Include(fr => fr.Studant).ThenInclude(s => s.User)
            .Where(fr => fr.StudantId == studantId && fr.FormationId == formationId && fr.Situation == Constants.SituationsObjects.NormalSituation);

            var request = await query.FirstOrDefaultAsync();

            if (request is null) return request;
            request.Formation.FormationEvents = request.Formation.FormationEvents.Where(fe => fe.State == FormationEventStates.Waiting.ToString("g")
            && fe.Situation == Constants.SituationsObjects.NormalSituation).ToList();

            return request;
        }

        private CreationResult<FormationRequest> FailCreation()
        {
            return new CreationResult<FormationRequest>
            {
                Succeded = false,
                Errors = new string[] { Constants.ModelMessages.FailModelCreated }
            };
        }


    }
}
