using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alumni.Contracts.V1;
using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Requests.Queries;
using alumni.Contracts.V1.Responses;
using alumni.Data;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers.PaginationHelpers;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace alumni.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SchoolCourseController : ControllerBase
    {
        private readonly IMapper mapper;

        private readonly IUriService uriService;

        private readonly ISchoolCourseService schoolCourseService;

        public SchoolCourseController(IMapper mapper, ISchoolCourseService schoolCourseService, IUriService uriService)
        {
            this.mapper = mapper;

            this.uriService = uriService;

            this.schoolCourseService = schoolCourseService;
        }

        [HttpPost(ApiRoutes.SchoolCourseRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] SchoolCoursesRequest schoolCouseRequest)
        {
            if (schoolCouseRequest == null) return BadRequest();

            var route = ApiRoutes.SchoolCourseRoutes.Get;

            var schoolCourse = mapper.Map<SchoolCourses>(schoolCouseRequest);

            var creationResult = await schoolCourseService.CreateAsync(schoolCourse);

            if (!creationResult.Succeded && creationResult.Exists) 
                return Conflict();
            else if (!creationResult.Succeded) return BadRequest();

            var parameter = new Dictionary<string, string>
                    {
                        {"{courseId}",creationResult.Data.CourseId },
                        {"{schoolId}",creationResult.Data.SchoolId },
                    };

            var creationResponse = new CreationResponse<SchoolCoursesResponse>
            {
                Data = mapper.Map<SchoolCoursesResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                GetUri = uriService.GetModelUri(parameter, route),
                Succeded = creationResult.Succeded
            };

            return Created(creationResponse.GetUri, creationResponse);
        }

        [HttpGet(ApiRoutes.SchoolCourseRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query, [FromQuery] SchoolCourseQuery param)
        {
            if (query == null) return BadRequest();

            var filter = mapper.Map<PaginationFilter>(query);

            var courses = await schoolCourseService.GetSchoolCoursesAsync(filter, param);

            var schoolCoursesResponse = mapper.Map<List<SchoolCoursesResponse>>(courses);

            var pageResponse = new PageResponse<SchoolCoursesResponse>(schoolCoursesResponse);

            if (filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var searchMode = filter.SearchValue != null;

            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                                                        response: schoolCoursesResponse,
                                                                        uriService: uriService,
                                                                        path: ApiRoutes.SchoolCourseRoutes.GetAll,
                                                                        searchMode: searchMode);

            return Ok(paginationResponse);
        }
    }
}
