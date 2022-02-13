
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class OrganRoutes
        {
            public const string OrganRoute = Base + "/organ";

            public const string Create = OrganRoute + "/create";

            public const string GetAll = OrganRoute + "/getAll";

            public const string Get = OrganRoute + "/get/{id}";
        }
        public static class AcademyRoutes
        {
            public const string AcademyRoute = Base + "/academy";

            public const string Create = AcademyRoute + "/create";

            public const string GetAll = AcademyRoute + "/getAll";

            public const string Get = AcademyRoute + "/get";

            public const string GetByName = AcademyRoute + "/getByName/{name}";
        }

        public static class AnswerRoutes
        {
            public const string AnswerRoute = Base + "/answer";

            public const string Create = AnswerRoute + "/create";

            public const string GetAll = AnswerRoute + "/getAll";

            public const string Get = AnswerRoute + "/get/{Id}";
        }

        public static class ArticleRoutes
        {
            public const string ArticleRoute = Base + "/article";

            public const string Create = ArticleRoute + "/create";

            public const string GetAll = ArticleRoute + "/getAll";

            public const string Get = ArticleRoute + "/get/{Id}";

            public const string Update = ArticleRoute + "/update/{articleId}";
        }

        public static class BadgeInformationRoutes
        {
            public const string BadgeInformationRoute = Base + "/badgeInformation";

            public const string Create = BadgeInformationRoute + "/create";

            public const string Get = BadgeInformationRoute + "/get/{Id}";
        }

        public static class CommentableRoutes
        {

        }

        public static class CommentRoutes
        {
            public const string CommentRoute = Base + "/comment";

            public const string Create = CommentRoute + "/create";
            
            public const string Get = CommentRoute + "/get/{Id}";

            public const string GetAll = CommentRoute + "/getAll";
        }

        public static class CourseRoutes
        {
            public const string CourseRoute = Base + "/course";

            public const string Create = CourseRoute + "/create";

            public const string GetAll = CourseRoute + "/getAll";

            public const string Get = CourseRoute + "/get";
        }

        public static class DisciplineRoutes
        {
            public const string DisciplineRoute = Base + "/discipline";

            public const string Create = DisciplineRoute + "/create";

            public const string GetAll = DisciplineRoute + "/getAll";

            public const string Get = DisciplineRoute + "/get/{Id}";
        }

        public static class DisciplineTopicRoutes
        {
            public const string DisciplineTopicRoute = Base + "/disciplineTopic";

            public const string Create = DisciplineTopicRoute + "/create";

            public const string GetAll = DisciplineTopicRoute + "/getAll";

            public const string Get = DisciplineTopicRoute + "/get/{Id}";
        }

        public static class LessonRoutes
        {
            public const string LessonRoute = Base + "/lesson";

            public const string Create = LessonRoute + "/create";

            public const string GetAll = LessonRoute + "/getAll";

            public const string GetAllByStudant = LessonRoute + "/getAll/studant/{studantId}";

            public const string GetAllByTeacherPlace = LessonRoute + "/getAll/teacherPlace/{teacherPlaceId}";

            public const string Get = LessonRoute + "/get/{Id}";

            public const string GetByPost = LessonRoute + "/get/post/{postId}";
        }

        public static class ManagerRoutes
        {
            public const string ManagerRoute = Base + "/manager";

            public const string Create = ManagerRoute + "/create";

            public const string Get = ManagerRoute + "/get/{id}";

            public const string GetByUser = ManagerRoute + "/getByUser/{userId}";
        }

        public static class PostRoutes
        {
            public const string PostRoute = Base + "/post";

            public const string Create = PostRoute + "/create";

            public const string GetAll = PostRoute + "/getAll";

            public const string Update = PostRoute + "/update/{Id}";

            public const string Get = PostRoute + "/get/{Id}";
        }

        public static class QuestionRoutes
        {
            public const string QuestionRoute = Base + "/question";

            public const string Create = QuestionRoute + "/create";

            public const string GetAll = QuestionRoute + "/getAll";
            
            public const string Patch = QuestionRoute + "/patch/{Id}";

            public const string GetAllByStudant = QuestionRoute + "/getAll/studant/{studantId}";
            
            public const string GetAllByTeacherPlace= QuestionRoute + "/getAll/teacherPlace/{teacherPlaceId}";

            public const string Update = QuestionRoute + "/update/{Id}";

            public const string Get = QuestionRoute + "/get/{Id}";

            public const string GetByPost = QuestionRoute + "/get/post/{postId}";
        }

        public static class SchoolCourseRoutes
        {
            public const string SchoolCourseRoute = Base + "/schoolCourse";

            public const string Create = SchoolCourseRoute + "/create";

            public const string Get = SchoolCourseRoute + "/get/{schoolId}/{courseId}";
            
            public const string GetAll = SchoolCourseRoute + "/getAll";
        }

        public static class SchoolRoutes
        {
            public const string SchoolRoute = Base + "/school";

            public const string Create = SchoolRoute + "/create";

            public const string Get = SchoolRoute + "/get/{Id}";            

            public const string GetAll = SchoolRoute + "/getAll";
        }

        public static class SchoolIdentityRoutes
        {
            public const string SchoolIdentityRoute = Base + "/schoolIdentity";

            public const string Create = SchoolIdentityRoute + "/create";

            public const string Get = SchoolIdentityRoute + "/get/{Id}";
        }

        public static class StudantRoutes
        {
            public const string StudantRoute = Base + "/studant";

            public const string Create = StudantRoute + "/create";

            public const string GetAll = StudantRoute + "/getAll";

            public const string Update = StudantRoute + "/update/{Id}";

            public const string Get = StudantRoute + "/get/{Id}";
        }

        public static class UserRoutes
        {
            public const string UserRoute = Base + "/user";

            public const string Login = UserRoute + "/login";

            public const string Registration = UserRoute + "/registration";

            public const string Refresh = UserRoute + "/refresh";

            public const string Get = UserRoute + "/get/{id}";

            public const string GetTeacher = UserRoute + "/get/teacher/{id}";

            public const string GetStudant = UserRoute + "/get/studant/{id}";

            public const string GetAll = UserRoute + "/getAll";
        }

        public static class TeacherRoutes
        {
            public const string TeacherRoute = Base + "/teacher";

            public const string Create = TeacherRoute + "/create";

            public const string GetAll = TeacherRoute + "/getAll";

            public const string Get = TeacherRoute + "/get/{Id}";

            public const string GetByUser = TeacherRoute + "/getByUser/{userId}";
        }

        public static class TeacherSchoolsRoutes
        {
            public const string TeacherSchoolsRoute = Base + "/teacherSchools";

            public const string Create = TeacherSchoolsRoute + "/create";

            public const string Update = TeacherSchoolsRoute + "/update/{teacherId}/{schoolId}";

            public const string GetAll = TeacherSchoolsRoute + "/getAll";           

            public const string CheckTeacherHasSchool = TeacherSchoolsRoute + "/checkTeacherHasSchool/{teacherId}";

        }

        public static class TeacherPlaceRoutes
        {
            public const string TeacherPlaceRoute = Base + "/teacherPlace";

            public const string Create = TeacherPlaceRoute + "/create";

            public const string GetAll = TeacherPlaceRoute + "/getAll";

            public const string Get = TeacherPlaceRoute + "/get/{Id}";
        }

        public static class TeacherPlaceMaterialRoutes
        {
            public const string TeacherPlaceMaterialRoute = Base + "/teacherPlaceMaterial";

            public const string Create = TeacherPlaceMaterialRoute + "/create";

            public const string GetAllByTeacherPlace = TeacherPlaceMaterialRoute + "/getAll/{teacherPlaceId}";

            public const string Get = TeacherPlaceMaterialRoute + "/get/{Id}";
            
            public const string GetByPost = TeacherPlaceMaterialRoute + "/get/post/{postId}";
        }

        public static class TeacherPlaceMessageRoutes
        {
            public const string TeacherPlaceMessageRoute = Base + "/teacherPlaceMessage";

            public const string Create = TeacherPlaceMessageRoute + "/create";

            public const string GetAllByTeacherPlace = TeacherPlaceMessageRoute + "/getAll/{teacherPlaceId}";

            public const string Get = TeacherPlaceMessageRoute + "/get/{Id}";
            
            public const string GetByPost = TeacherPlaceMessageRoute + "/get/post/{postId}";
        }

        public static class TeacherPlaceStudantsRoutes
        {
            public const string TeacherPlaceStudantsRoute = Base + "/teacherPlaceStudants";

            public const string Create = TeacherPlaceStudantsRoute + "/create";

            public const string Update = TeacherPlaceStudantsRoute + "/update/{teacherPlaceId}/{studantId}";

            public const string Get = TeacherPlaceStudantsRoute + "/get/{teacherPlaceId}/{studantId}";

            public const string GetAllTeacherPlaceByStudant = TeacherPlaceStudantsRoute + "/getAll/teacherPlace/{studantId}";

            public const string GetAllStudantsByTeacherPlace = TeacherPlaceStudantsRoute + "/getAll/studants/{teacherPlaceId}";
        }

        public static class TopicRoutes
        {
            public const string TopicRoute = Base + "/topic";

            public const string Create = TopicRoute + "/create";

            public const string Update = TopicRoute + "/update/{Id}";

            public const string Get = TopicRoute + "/get/{Id}";

            public const string GetAll = TopicRoute + "/getAll";
        }

        public static class ImagesRoutes
        {
            public const string ImageRoute = Base + "/image";
            
            public const string UploadLessonProfile = ImageRoute + "/upload/lesson/profile";                        
            public const string UploadTopicImage = ImageRoute + "/upload/topic";                        
        }

        public static class VideoRoutes
        {
            public const string VideoRoute = Base + "/video";

            public const string Create = VideoRoute + "/create";

            public const string Upload = VideoRoute + "/upload";

            public const string GetAll = VideoRoute + "/getAll";

            public const string Get = VideoRoute + "/get/{Id}";

            public const string VideoWatch = VideoRoute + "/upload/watch/{connectionId}";
        }
    }
}
