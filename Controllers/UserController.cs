using System.Collections.Generic;
using System.Threading.Tasks;
using alumni.Contracts.V1;
using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Requests.Queries;
using alumni.Contracts.V1.Responses;
using alumni.Data;
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
    public class UserController : Controller
    {
        private readonly DataContext dataContext;

        private readonly IUserService userService;

        private readonly IMapper mapper;

        private readonly IUriService uriService;

        public UserController(DataContext dataContext, IUserService userService, IMapper mapper, IUriService uriService)
        {
            this.dataContext = dataContext;

            this.userService = userService;

            this.mapper = mapper;

            this.uriService = uriService;
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.UserRoutes.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null) return BadRequest();

            var login = mapper.Map<LoginDomain>(loginRequest);

            if (login.GrantType == Constants.UserContansts.GrantPassword)
            {
                var auth = await userService.LoginAsync(login);

                var authResponse = mapper.Map<AuthResultResponse>(auth);

                if (!authResponse.Authenticated) return Conflict(authResponse);

                authResponse.User.Role = await userService.GetRoleAsync(authResponse.User.Id);

                return Ok(authResponse);
            }

            if (login.GrantType == Constants.UserContansts.GrantRefresh)
            {
                var auth = await userService.RefreshTokenAsync(login.Token, login.RefreshToken);

                var authResponse = mapper.Map<AuthResultResponse>(auth);

                if (!authResponse.Authenticated)
                    return Conflict(authResponse);

                authResponse.User.Role = await userService.GetRoleAsync(authResponse.User.Id);

                return Ok(authResponse);
            }



            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.UserRoutes.Registration)]
        public async Task<IActionResult> Registration([FromBody] CreateUserRequest registrationRequest)
        {
            if (registrationRequest == null) return BadRequest();

            var registration = mapper.Map<RegistrationDomain>(registrationRequest);

            var auth = await userService.RegistrationAsync(registration);

            var authResponse = mapper.Map<AuthResultResponse>(auth);

            authResponse.User.Role = registration.Role;

            return Ok(authResponse);
        }

        [HttpGet(ApiRoutes.UserRoutes.Get)]
        public async Task<IActionResult> GetUser([FromRoute] string Id)
        {
            var user = await userService.GetUserAsync(Id);

            if (user == null) return NotFound();

            var userResponse = mapper.Map<UserResponse>(user);

            var response = new Response<UserResponse>(userResponse);

            return Ok(response);
        }

        [HttpGet(ApiRoutes.UserRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
        {
            var filter = mapper.Map<PaginationFilter>(query);

            var users = await userService.GetUsersAsync(filter);

            var usersResponse = mapper.Map<List<UserResponse>>(users);

            var pageResponse = new PageResponse<UserResponse>(usersResponse);

            if (filter == null || filter.PageNumber < 1 || filter.PageSize < 1)
                return Ok(pageResponse);

            var path = "api/v1/user/getUsers";

            var searchMode = filter.SearchValue != null;


            var paginationResponse = PaginationHelpers.CreatePaginationResponse(paginationFilter: filter,
                                                                            response: usersResponse,
                                                                            uriService: uriService,
                                                                            path: path,
                                                                            searchMode: searchMode);

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.UserRoutes.GetTeacher)]
        public async Task<IActionResult> GetTeacher([FromRoute] string Id)
        {
            if (Id == null) return BadRequest();

            var teacher = await userService.GetTeacherAsync(Id);

            var teacherResponse = mapper.Map<TeacherResponse>(teacher);

            return Ok(new Response<TeacherResponse>(teacherResponse));
        }

        [HttpGet(ApiRoutes.UserRoutes.GetStudant)]
        public async Task<IActionResult> GetStudant([FromRoute] string Id)
        {
            if (Id == null) return BadRequest();

            var school = await userService.GetStudantAsync(Id);

            var schoolResponse = mapper.Map<StudantResponse>(school);

            return Ok(new Response<StudantResponse>(schoolResponse));
        }
    }
}
