using System;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace alumni.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class SubscriptionController  : ControllerBase
    {
        private readonly ISubscriptionService service;

        private readonly IMapper mapper;

        private readonly IUriService uriService;


        public SubscriptionController (ISubscriptionService studantService, IMapper mapper, IUriService uriService)
        {
            this.service = studantService;

            this.mapper = mapper;

            this.uriService = uriService;
        }
     
        [HttpGet(ApiRoutes.SubscriptionRoutes.Get)]
        public async Task<IActionResult> Get([FromRoute] string studantId, [FromRoute] string formationId)
        {
            if (studantId is null || formationId is null) return BadRequest();

            var subscription = await service.GetAsync(studantId, formationId);

            if (subscription == null) return BadRequest();

            var response = new Response<SubscriptionResponse>(mapper.Map<SubscriptionResponse>(subscription));

            return Ok(response);
        }      

        [HttpGet(ApiRoutes.SubscriptionRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery page, [FromQuery] SubscriptionQuery query)
        {
            if (query == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(page);

            var searchMode = filter.SearchValue != null;

            var pageResult = await service.GetSubscriptionsAsync(filter, query);            

            var pageResponse = new PageResponse<SubscriptionResponse> { 
                Data = mapper.Map<IEnumerable<Subscription>, IEnumerable<SubscriptionResponse>>(pageResult.Data),
                TotalElements = pageResult.TotalElements
            }; 

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                    response: pageResponse.Data,
                                    uriService: uriService,
                                    path: ApiRoutes.SubscriptionRoutes.GetAll,
                                    searchMode: searchMode);

            paginationResponse.TotalElements = pageResult.TotalElements;

            return Ok(paginationResponse);
        }          
    }
}
