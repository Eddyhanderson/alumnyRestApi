using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alumni.Contracts.V1;
using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Requests.Queries;
using alumni.Contracts.V1.Responses;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using Alumni.Helpers.PaginationHelpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace alumni.Controllers
{
    public class FormationController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IFormationService service;

        private readonly IUriService uriService;
        public FormationController(IMapper mapper, IFormationService service, IUriService uriService)
        {
            this.mapper = mapper;

            this.service = service;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.FormationRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] CreateFormationRequest formationRequest)
        {
            if (formationRequest is null) return BadRequest();
            var formation = mapper.Map<Formation>(formationRequest);

            var creationResult = await service.CreateAsync(formation);

            if (!creationResult.Succeded) return Conflict(creationResult);

            var parameter = new Dictionary<string, string>{
                    { "{id}", creationResult.Data.Id}
                };

            var creationResponse = new CreationResponse<FormationCreationResponse>
            {
                Data = mapper.Map<FormationCreationResponse>(creationResult.Data),
                GetUri = uriService.GetModelUri(parameter, ApiRoutes.FormationRoutes.Get),
                Succeded = true
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.FormationRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery pagQuery, [FromQuery] FormationQuery query)
        {
            var filter = mapper.Map<PaginationFilter>(pagQuery);

            var searchMode = filter.SearchValue != null;

            var pageResult = await service.GetFormationsAsync(filter, query);

            var pageResponse = new PageResponse<FormationResponse>
            {
                Data = mapper.Map<IEnumerable<Formation>, IEnumerable<FormationResponse>>(pageResult.Data),
                TotalElements = pageResult.TotalElements
            };

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: pageResponse.Data,
                                    uriService: uriService,
                                    path: ApiRoutes.FormationRoutes.GetAll,
                                    searchMode: searchMode);

            paginationResponse.TotalElements = pageResponse.TotalElements;

            return Ok(paginationResponse);
        }

        [Authorize(Roles = Constants.UserContansts.StudantRole)]
        [Route(ApiRoutes.FormationRoutes.GetAllPublished)]
        public async Task<IActionResult> GetAllPublished([FromQuery] PaginationQuery page, [FromQuery] FormationQuery query)
        {
            var filter = mapper.Map<PaginationFilter>(page);

            var searchMode = filter.SearchValue != null;

            var pageResult = await service.GetPublishedFormationsAsync(filter, query);

            var pageResponse = new PageResponse<FormationResponse>
            {
                Data = mapper.Map<IEnumerable<Formation>, IEnumerable<FormationResponse>>(pageResult.Data),
                TotalElements = pageResult.TotalElements
            };

            pageResponse.Data.ToList().ForEach(fr => {
                var formation = pageResult.Data.FirstOrDefault(f => f.Id == fr.Id);
                SetResponseData(formation, fr);
            });
            
            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: pageResponse.Data,
                                    uriService: uriService,
                                    path: ApiRoutes.FormationRoutes.GetAll,
                                    searchMode: searchMode);

            paginationResponse.TotalElements = pageResponse.TotalElements;

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.FormationRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            if (id != null)
            {
                var formation = await service.GetFormationAsync(id);

                if (formation == null) return NotFound();                

                var response = new Response<FormationResponse>(mapper.Map<FormationResponse>(formation));

                SetResponseData(formation, response.Data);

                return Ok(response);
            }

            return BadRequest();
        }
        private FormationResponse SetResponseData(Formation formation, FormationResponse response)
        {
            var lessonCount = 0;
            formation.Modules.ForEach(m => lessonCount += m.Lessons.Count);
            response.ModulesCount = formation.Modules.Count;
            response.LessonCount = lessonCount;
            response.SubscriptionCount = formation.FormationEvents[0].Subscriptions.Count;
            response.State = formation.FormationEvents[0].State;
            response.Start = formation.FormationEvents[0].Start;
            response.End = formation.FormationEvents[0].End;
            response.StudantLimit = formation.FormationEvents[0].StudantLimit;

            return response;
        }
    }
}