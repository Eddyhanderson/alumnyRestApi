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
using Alumni.Helpers.PaginationHelpers;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace alumni.Controllers
{
    [ApiController]
    public class TeacherSchoolsController : ControllerBase
    {
        private readonly ITeacherSchoolsService teacherSchoolsService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;

        public TeacherSchoolsController(ITeacherSchoolsService teacherSchollsService,
            IMapper mapper, IUriService uriService)
        {
            this.teacherSchoolsService = teacherSchollsService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.TeacherSchoolsRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] TeacherSchoolsRequest teacherSchoolsRequest)
        {
            if (teacherSchoolsRequest == null) return BadRequest();

            var teacherSchools = mapper.Map<TeacherSchools>(teacherSchoolsRequest);

            var creationResult = await teacherSchoolsService.CreateAsync(teacherSchools);

            if (!creationResult.Succeded) return Conflict();

            var creationResponse = new CreationResponse<TeacherSchoolsResponse>
            {
                Data = mapper.Map<TeacherSchoolsResponse>(creationResult.Data),
                Errors = creationResult.Errors,
                Messages = creationResult.Messages,
                Succeded = creationResult.Succeded
            };

            return Created("", creationResponse);
        }

        [HttpPut(ApiRoutes.TeacherSchoolsRoutes.Update)]
        public async Task<IActionResult> Update([FromRoute] string teacherId, [FromRoute] string schoolId, [FromBody] TeacherSchoolsRequest teacherSchoolsRequest)
        {
            var teacherSchool = mapper.Map<TeacherSchools>(teacherSchoolsRequest);

            var updated = await teacherSchoolsService.UpdateAsync(teacherId, schoolId, teacherSchool);

            if (updated) return NoContent();

            return BadRequest();
        }

        [HttpGet(ApiRoutes.TeacherSchoolsRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery pQuery, [FromQuery] TeacherSchoolQuery tQuery)
        {
            var filter = mapper.Map<PaginationFilter>(pQuery);

            var teacherSchools = await teacherSchoolsService.GetAllAsync(filter, tQuery);

            var teacherSchoolsResponse = mapper.Map<List<TeacherSchoolsResponse>>(teacherSchools);

            if (filter == null || filter.PageNumber < 1 || filter.PageSize < 1)
            {
                var pageResponse = new PageResponse<TeacherSchoolsResponse>(teacherSchoolsResponse);

                return Ok(pageResponse);
            }

            var path = ApiRoutes.TeacherSchoolsRoutes.GetAll;

            var searchMode = filter.SearchValue != null;

            var pageResponseK = PaginationHelpers
                .CreatePaginationResponse(filter, teacherSchoolsResponse, uriService, path, searchMode);

            return Ok(pageResponseK);
        }

        [HttpGet(ApiRoutes.TeacherSchoolsRoutes.CheckTeacherHasSchool)]
        public async Task<IActionResult> CheckTeacherHasSchool([FromRoute] string teacherId)
        {
            if (teacherId == null) return BadRequest();

            var checkResult = await teacherSchoolsService.CheckTeacherHasSchoolAsync(teacherId);

            var response = new Response<bool>(checkResult);

            return Ok(response);
        }
    }
}
