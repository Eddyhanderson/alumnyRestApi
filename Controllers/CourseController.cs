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
using Microsoft.AspNetCore.Mvc;

namespace alumni.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IMapper mapper;

        private readonly IUriService uriService;

        private readonly ICourseService courseService;


        public CourseController(ICourseService courseService, IMapper mapper, IUriService uriService)
        {
            this.courseService = courseService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.CourseRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] CourseRequest courseRequest)
        {
            if (courseRequest != null)
            {
                string route = ApiRoutes.CourseRoutes.Get;

                var course = mapper.Map<Course>(courseRequest);

                var creationResult = await courseService.CreateAsync(course);

                if (!creationResult.Succeded) return Conflict();

                var parameter = new Dictionary<string, string>
                    {
                        {"{Id}",creationResult.Data.Id }
                    };

                var creationResponse = new CreationResponse<CourseResponse>
                {
                    Data = mapper.Map<CourseResponse>(creationResult.Data),
                    Errors = creationResult.Errors,
                    Messages = creationResult.Messages,
                    GetUri = uriService.GetModelUri(parameter, route),
                    Succeded = creationResult.Succeded
                };

                return Created(creationResponse.GetUri, creationResponse);
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.CourseRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query, [FromQuery] CourseQuery param)
        {
            if (query != null)
            {
                var filter = mapper.Map<PaginationFilter>(query);

                var searchMode = filter.SearchValue != null;

                var courses = await courseService.GetCoursesAsync(filter);

                var coursesResponse = mapper.Map<List<CourseResponse>>(courses);

                var pageResponse = new PageResponse<CourseResponse>(coursesResponse);

                if (filter.PageNumber < 1 || filter.PageSize < 1)
                    return Ok(pageResponse);

                var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                                                            response: coursesResponse,
                                                                            uriService: uriService,
                                                                            path: ApiRoutes.CourseRoutes.GetAll,
                                                                            searchMode: searchMode);

                return Ok(paginationResponse);
            }
            return BadRequest();
        }

        [HttpGet(ApiRoutes.CourseRoutes.Get)]
        public async Task<IActionResult> Get([FromQuery] CourseQuery query)
        {
            if (query != null)
            {
                var course = await courseService.GetCourseAsync(query);

                if (course == null) return NotFound();

                var response = new Response<CourseResponse>(mapper.Map<CourseResponse>(course));

                return Ok(response);
            }
            return BadRequest();
        }        
    }
}
