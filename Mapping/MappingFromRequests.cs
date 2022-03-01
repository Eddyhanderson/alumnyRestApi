using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Mapping
{
    public class MappingFromRequests : Profile
    {
        public MappingFromRequests()
        {
            /*CreateMap<AcademyRequest, Academy>();

            CreateMap<AdminRequest, Admin>();

            CreateMap<AnswerRequest, Answer>();*/
            CreateMap<CreateFormationRequest, Formation>();

            CreateMap<FormationRequestRequest, FormationRequest>();

            CreateMap<FormationEventRequest, FormationEvent>();

            CreateMap<LoginRequest, LoginDomain>();
            /*
                CreateMap<RegistrationRequest, RegistrationDomain>();
            */
            CreateMap<PaginationQuery, PaginationFilter>();

            CreateMap<ArticleRequest, Article>();
            /*
                        CreateMap<BadgeInformationRequest, BadgeInformation>();

                        CreateMap<CommentRequest, Comment>();

                        CreateMap<CommentableRequest, Commentable>();

                        CreateMap<CourseRequest, Course>();

                        CreateMap<DisciplineRequest, Discipline>();

                        CreateMap<DisciplineTopicRequest, DisciplineTopic>();*/

            CreateMap<LessonRequest, Lesson>();

            CreateMap<CreateManagerRequest, Manager>();
            CreateMap<CreateManagerRequest, User>();
            CreateMap<CreateModuleRequest, Module>();

            /*
            CreateMap<NotificationRequest, Notification>();
            */
            CreateMap<OrganRequest, Organ>();

            /*

            CreateMap<PostRequest, Post>();

            CreateMap<QuestionRequest, Question>();

            */

            CreateMap<CreateSchoolRequest, School>();
            CreateMap<CreateSchoolRequest, User>();

            /*

            CreateMap<SchoolRequest, School>();

            CreateMap<SchoolCoursesRequest, SchoolCourses>();
            */
            CreateMap<CreateStudantRequest, Studant>();
            CreateMap<CreateStudantRequest, User>();

            /*

            CreateMap<SchoolIdentityCreation, SchoolIdentity>();            

            CreateMap<TeacherRequest, Teacher>();

            CreateMap<TeacherSchoolsRequest, TeacherSchools>();

            CreateMap<TeacherPlaceRequest, TeacherPlace>();

            CreateMap<TeacherPlaceStudantsRequest, TeacherPlaceStudants>();

            CreateMap<TeacherPlaceMaterialRequest, TeacherPlaceMaterial>();

            CreateMap<TeacherPlaceMessageRequest, TeacherPlaceMessage>();

            CreateMap<TopicRequest, Topic>();

            CreateMap<UserRequest, User>();
            */
            CreateMap<VideoRequest, Video>();
        }
    }
}
