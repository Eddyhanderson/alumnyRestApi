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
            CreateMap<Article, ArticleResponse>();

            CreateMap<AuthResult, AuthResultResponse>();

            CreateMap<AuthConfigTokens, AuthConfigTokensResponse>();

            CreateMap<Certificate, CertificateResponse>();

            CreateMap<Formation, FormationCreationResponse>();

            CreateMap<FormationEvent, FormationEventResponse>();

            CreateMap<Formation, FormationResponse>()
            .ForMember(fr => fr.SchoolPicture, m => m.MapFrom(f => f.School.User.Picture))
            .ForMember(fr => fr.SchoolAcronym, m => m.MapFrom(f => f.School.Acronym))
            .ForMember(fr => fr.SchoolName, m => m.MapFrom(f => f.School.Name));

            CreateMap<FormationRequest, FormationRequestResponse>()
            .ForMember(frr => frr.StudantLastName, m => m.MapFrom(fr => fr.Studant.LastName))
            .ForMember(frr => frr.StudantFisrtName, m => m.MapFrom(fr => fr.Studant.FirstName))
            .ForMember(frr => frr.StudantPicture, m => m.MapFrom(fr => fr.Studant.User.Picture))
            .ForMember(frr => frr.StudantOrganId, m => m.MapFrom(fr => fr.Studant.OrganId))
            .ForMember(frr => frr.StudantOrganName, m => m.MapFrom(fr => fr.Studant.Organ.Name))
            .ForMember(frr => frr.StudantOrganBadget, m => m.MapFrom(fr => fr.Studant.Organ.Badget))
            .ForMember(frr => frr.FormationPrice, m => m.MapFrom(fr => fr.Formation.Price))
            .ForMember(frr => frr.FormationSchoolPicture, m => m.MapFrom(fr => fr.Formation.School.User.Picture))
            .ForMember(frr => frr.FormationSchoolName, m => m.MapFrom(fr => fr.Formation.School.Name))
            .ForMember(frr => frr.FormationSchoolAcronym, m => m.MapFrom(fr => fr.Formation.School.Acronym))
            .ForMember(frr => frr.FormationTheme, m => m.MapFrom(fr => fr.Formation.Theme))
            .ForMember(frr => frr.FormationId, m => m.MapFrom(fr => fr.Formation.Id));

            CreateMap<Module, ModuleResponse>();

            CreateMap<Lesson, LessonResponse>()
            .ForMember(lr => lr.SchoolName, m => m.MapFrom(l => l.Module.Formation.School.Name))
            .ForMember(lr => lr.SchoolAcronymName, m => m.MapFrom(l => l.Module.Formation.School.Acronym))
            .ForMember(lr => lr.ModuleName, m => m.MapFrom(l => l.Module.Name))
            .ForMember(lr => lr.FormationTheme, m => m.MapFrom(l => l.Module.Formation.Theme))
            .ForMember(lr => lr.Date, m => m.MapFrom(l => l.Post.CreateAt))
            .ForMember(lr => lr.SchoolPicture, m => m.MapFrom(l => l.Module.Formation.School.User.Picture))
            .ForMember(lr => lr.Duration, m => m.MapFrom(l =>
                    l.Video.Duration.Substring(0, 2) == "00" ? l.Video.Duration.Substring(3) : l.Video.Duration));

            CreateMap<Manager, ManagerResponse>();


            CreateMap<Organ, OrganResponse>();
            /*
            CreateMap<Post, PostResponse>()
                .ForMember(pr => pr.UserPictureProfilePath, m => m.MapFrom(p => p.User.PictureProfilePath));
            */
            CreateMap<Subscription, SubscriptionResponse>()
                .ForMember(sr => sr.FormationEnd, m => m.MapFrom(s => s.FormationEvent.End))
                .ForMember(sr => sr.FormationStart, m => m.MapFrom(s => s.FormationEvent.Start))
                .ForMember(sr => sr.FormationId, m => m.MapFrom(s => s.FormationEvent.FormationId))
                .ForMember(sr => sr.FormationTheme, m => m.MapFrom(s => s.FormationEvent.Formation.Theme))
                .ForMember(sr => sr.FormationSchoolName, m => m.MapFrom(s => s.FormationEvent.Formation.School.Name))
                .ForMember(sr => sr.FormationSchoolPicture, m => m.MapFrom(s => s.FormationEvent.Formation.School.User.Picture))
                .ForMember(sr => sr.StudantName, m => m.MapFrom(s => $"{s.Studant.FirstName} {s.Studant.LastName}"))
                .ForMember(sr => sr.StudantPicture, m => m.MapFrom(s => s.Studant.User.Picture))
                .ForMember(sr => sr.StudantId, m => m.MapFrom(s => s.Studant.Id));

            /*

            CreateMap<RegisterInCourseRequest, RegisterInCourseRequestResponse>();

            CreateMap<RegisterInSchoolRequest, RegisterInSchoolRequestResponse>();

            CreateMap<SchoolCourses, SchoolCoursesResponse>();*/


            CreateMap<School, SchoolResponse>();

            CreateMap<Studant, StudantResponse>()
            .ForMember(sr => sr.OrganName, m => m.MapFrom(s => s.Organ.Name))
            .ForMember(sr => sr.Picture, m => m.MapFrom(s => s.User.Picture));

            CreateMap<User, UserResponse>();
            CreateMap<Video, VideoResponse>();
        }
    }
}
