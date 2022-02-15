using alumni.Contracts.V1.Responses;
using alumni.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Mapping
{
    public class MappingFromDomain : Profile
    {
        public MappingFromDomain()
        {
            /*CreateMap<Academy, AcademyResponse>();

            CreateMap<Answer, AnswerResponse>()
                .ForMember(ar => ar.UserPhoto, m => m.MapFrom(a => a.Post.User.PictureProfilePath))
                .ForMember(ar => ar.CreateAt, m => m.MapFrom(a => a.Post.CreateAt))
                .ForMember(ar => ar.UserId, m => m.MapFrom(a => a.Post.UserId))
                .ForMember(ar => ar.UserId, m => m.MapFrom(a => a.Post.UserId))
                .ForMember(ar => ar.CommentableId, m => m.MapFrom(a => a.Post.CommentableId));

            CreateMap<AcademicLevel, AcademicLevelResponse>();

            CreateMap<Admin, AdminResponse>();            

            CreateMap<Article, ArticleResponse>();

            */
            CreateMap<AuthResult, AuthResultResponse>();

            CreateMap<AuthConfigTokens, AuthConfigTokensResponse>();

            CreateMap<Formation, FormationCreationResponse>();
            
            CreateMap<Formation, FormationResponse>();

            CreateMap<Module, ModuleResponse>();
            /*
            CreateMap<BadgeInformation, BadgeInformationResponse>();

            CreateMap<Comment, CommentResponse>()
                .ForMember(cr => cr.UserPhoto, m => m.MapFrom(c => c.Post.User.PictureProfilePath))
                .ForMember(cr => cr.CreateAt, m => m.MapFrom(c => c.Post.CreateAt));

            CreateMap<Commentable, CommentableResponse>();

            CreateMap<Course, CourseResponse>()
                .ForMember(cr => cr.ProfilePhotoPath, m => m.MapFrom(c => c.BadgeInformation.ProfilePhotoPath));

            CreateMap<Discipline, DisciplineResponse>();

            CreateMap<DisciplineTopic, DisciplineTopicResponse>();                

            CreateMap<Lesson, LessonResponse>()
                .ForMember(lr => lr.DisciplineTopicName, m => m.MapFrom(l => l.Topic.DisciplineTopic.Name))
                .ForMember(lr => lr.DiscpilineId, m => m.MapFrom(l => l.TeacherPlace.DisciplineId))
                .ForMember(lr => lr.DisciplineName, m => m.MapFrom(l => l.TeacherPlace.Discipline.Name))
                .ForMember(lr => lr.DisciplineTopicName, m => m.MapFrom(l => l.Topic.DisciplineTopic.Name))
                .ForMember(lr => lr.TeacherPlaceName, m => m.MapFrom(l => l.TeacherPlace.Name))               
                .ForMember(lr => lr.TeacherId, m => m.MapFrom(l => l.TeacherPlace.TeacherId))
                .ForMember(lr => lr.Date, m => m.MapFrom(l => l.Post.CreateAt))
                .ForMember(lr => lr.SchoolName, m => m.MapFrom(l => l.TeacherPlace.School.ShortName))
                .ForMember(lr => lr.TeacherPlacePhotoPath, m => m.MapFrom(l => l.TeacherPlace.ProfilePhotoPath))
                .ForMember(lr => lr.ManifestPath, m => m.MapFrom(l => l.Video.ManifestPath))
                .ForMember(lr => lr.SchoolId, m => m.MapFrom(l => l.TeacherPlace.School.Id))
                .ForMember(lr => lr.Duration, m => m.MapFrom(l =>
                    // For just send the duration with hour, only when grather than 0
                    l.Video.Duration.Substring(0,2) == "00" ? l.Video.Duration.Substring(3) : l.Video.Duration
                ));
        */
            CreateMap<Manager, ManagerResponse>();

            /*
            CreateMap<Notification, NotificationResponse>();
            */
            CreateMap<Organ, OrganResponse>();
            /*
            CreateMap<Post, PostResponse>()
                .ForMember(pr => pr.UserPictureProfilePath, m => m.MapFrom(p => p.User.PictureProfilePath));

            CreateMap<Question, QuestionResponse>()
                .ForMember(qr => qr.LessonBackgroundPhotoPath, m => m.MapFrom(q => q.Lesson.BackgroundPhotoPath))
                .ForMember(qr => qr.LessonSequence, m => m.MapFrom(q => q.Lesson.Sequence))
                .ForMember(qr => qr.LessonTitle, m => m.MapFrom(q => q.Lesson.Title))
                .ForMember(qr => qr.LessonType, m => m.MapFrom(q => q.Lesson.LessonType))
                .ForMember(qr => qr.StudantPhoto, m => m.MapFrom(q => q.Studant.User.PictureProfilePath))
                .ForMember(qr => qr.CreateAt, m => m.MapFrom(q => q.Post.CreateAt))
                .ForMember(qr => qr.CommentableId, m => m.MapFrom(q => q.Post.CommentableId));                

            CreateMap<RegisterInCourseRequest, RegisterInCourseRequestResponse>();

            CreateMap<RegisterInSchoolRequest, RegisterInSchoolRequestResponse>();

            CreateMap<SchoolCourses, SchoolCoursesResponse>();*/


            CreateMap<School, SchoolResponse>();
            /*

            CreateMap<School, SchoolResponse>()
                .ForMember(sr => sr.ProfilePhotoPath, m => { m.MapFrom(s => s.BadgeInformation.ProfilePhotoPath); });

            CreateMap<Studant, StudantResponse>()
                .ForMember(sr => sr.PictureProfilePath, m =>
                {
                    m.MapFrom(vr => vr.User.PictureProfilePath);
                });


            CreateMap<TeacherSchools, TeacherSchoolsResponse>();

            CreateMap<TeacherPlace, TeacherPlaceResponse>()
                .ForMember(tpr => tpr.SchoolName, m =>
                { 
                    m.MapFrom(tp => tp.School.Name);
                })
                .ForMember(tpr => tpr.SchoolPictureProfilePath, m =>
                { 
                    m.MapFrom(tp => tp.School.BadgeInformation.ProfilePhotoPath);
                }).ForMember(tpr => tpr.SchoolShortName, m =>
                {
                    m.MapFrom(tp => tp.School.ShortName);
                })
                .ForMember(tpr => tpr.CourseName, m =>
                {
                    m.MapFrom(tp => tp.Course.Name);
                })
                .ForMember(tpr => tpr.DisciplineName, m =>
                {
                    m.MapFrom(tp => tp.Discipline.Name);
                })
                .ForMember(tpr => tpr.TeacherPictureProfilePath, m =>
                {
                    m.MapFrom(tp => tp.Teacher.User.PictureProfilePath);
                });


            CreateMap<TeacherPlaceStudants, TeacherPlaceStudantsResponse>();

            CreateMap<TeacherPlaceMaterial, TeacherPlaceMaterialResponse>();

            CreateMap<TeacherPlaceMessage, TeacherPlaceMessageResponse>();

            CreateMap<Topic, TopicResponse>()
            .ForMember(tr => tr.DisciplineTopicName, m =>
            {
                m.MapFrom(t => t.DisciplineTopic.Name);
            })
            .ForMember(tr => tr.CommentableId, m =>
            {
                m.MapFrom(t => t.Post.CommentableId);
            })
            .ForMember(tr => tr.CreationAt, m =>
            {
                m.MapFrom(t => t.Post.CreateAt);
            })
            .ForMember(tr => tr.TeacherPlaceName, m =>
            {
                m.MapFrom(t => t.TeacherPlace.Name);
            })
            .ForMember(tr => tr.TeacherPlaceProfilePhoto, m =>
            {
                m.MapFrom(t => t.TeacherPlace.ProfilePhotoPath);
            })            
            .ForMember(tr => tr.DisciplineName, m =>
            {
                m.MapFrom(t => t.TeacherPlace.Discipline.Name);
            });
            */
            CreateMap<User, UserResponse>();

            /*
            CreateMap<Video, VideoResponse>();*/
        }
    }
}
