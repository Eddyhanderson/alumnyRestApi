using alumni.IServices;
using alumni.Options;
using alumni.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using AutoMapper;
using alumni.Domain;
using Microsoft.AspNetCore.Authorization;
using alumni.BackgroundServices;
using System.Threading.Channels;
using alumni.QueueProcessing;
using System.Text.Json.Serialization;

namespace alumni.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            #region(singleton)

            var tokenOptions = new TokenOptions();

            configuration.Bind(nameof(TokenOptions), tokenOptions);

            services.AddSingleton(tokenOptions);

            services.AddSingleton<IQueueBackgroundServices, QueueBackgroundServices>();

            services.AddSingleton<IUriService>(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();

                var request = accessor.HttpContext.Request;

                var basePath = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");

                return new UriService(basePath);
            });


            #endregion

            #region(scoped)

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IArticleService, ArticleService>();

            services.AddScoped<IAcademyService, AcademyService>();

            services.AddScoped<IAcademicLevelService, AcademicLevelService>();

            services.AddScoped<IAnswerService, AnswerService>();

            services.AddScoped<IBadgeInformationService, BadgeInformationService>();

            services.AddScoped<ICourseService, CourseService>();

            services.AddScoped<ICommentableService, CommentableService>();

            services.AddScoped<ICommentService, CommentService>();

            services.AddScoped<IDisciplineService, DisciplineService>();

            services.AddScoped<IDisciplineTopicService, DisciplineTopicService>();

            services.AddScoped<IFormationService, FormationService>();

            services.AddScoped<ILessonService, LessonService>();

            services.AddScoped<IManagerService, ManagerService>();

            services.AddScoped<IOrganService, OrganService>();

            services.AddScoped<IPostService, PostService>();

            services.AddScoped<ITopicService, TopicService>();

            services.AddScoped<IQuestionService, QuestionService>();

            services.AddScoped<ISchoolDeprecatedService, SchoolServiceDeprecated>();            

            services.AddScoped<ISchoolService, SchoolService>();  

            services.AddScoped<IStudantService, StudantService>();

            services.AddScoped<ITeacherPlaceStudantService, TeacherPlaceStudantService>();

            services.AddScoped<ITeacherPlaceMaterialService, TeacherPlaceMaterialService>();

            services.AddScoped<ITeacherPlaceMessageService, TeacherPlaceMessageService>();

            services.AddScoped<ITeacherPlaceService, TeacherPlaceService>();

            services.AddScoped<ITeacherSchoolsService, TeacherSchoolsService>();

            services.AddScoped<ITeacherService, TeacherService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IVideoService, VideoService>();

            #endregion

            #region(default)

            services.AddControllersWithViews().AddNewtonsoftJson(); 

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddAuthentication(
                    options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    }).AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = tokenOptions.Key,
                            ValidAudience = tokenOptions.Audience,
                            ValidIssuer = tokenOptions.Issuer,
                            ClockSkew = tokenOptions.ClockSkew,
                            ValidateLifetime = tokenOptions.ValidateLifetime,
                            ValidateAudience = tokenOptions.ValidateAudience,
                            ValidateIssuer = tokenOptions.ValidateIssuer,
                        };
                    });           

            services.AddHostedService<QueueTaskExecutionBackgroundService>();            

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://localhost")
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod();
                });
            });

            services.AddSignalR().AddMessagePackProtocol();


            /*services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());

                 auth.AddPolicy("TagsView", options =>
                 {
                     options.RequireClaim("tag-viwer", "true").Build();
                 });

                 auth.AddPolicy("EmailEndWith", options =>
                 {
                     options.AddRequirements(new WorkForCompanyRequiremt(".ao"));
                 });
            });*/

            #endregion
        }
    }
}
