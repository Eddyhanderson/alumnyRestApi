
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

        public static class AnswerRoutes
        {
            public const string AnswerRoute = Base + "/answer";

            public const string Create = AnswerRoute + "/create";

            public const string GetAll = AnswerRoute + "/getAll";

            public const string Get = AnswerRoute + "/get/{id}";
        }

        public static class ArticleRoutes
        {
            public const string ArticleRoute = Base + "/article";

            public const string Create = ArticleRoute + "/create";

            public const string GetAll = ArticleRoute + "/getAll";

            public const string Get = ArticleRoute + "/get/{id}";

            public const string Update = ArticleRoute + "/update/{articleId}";
        }

        public static class CommentableRoutes
        {

        }

        public static class CommentRoutes
        {
            public const string CommentRoute = Base + "/comment";

            public const string Create = CommentRoute + "/create";

            public const string Get = CommentRoute + "/get/{id}";

            public const string GetAll = CommentRoute + "/getAll";
        }
        public static class FormationRoutes
        {
            public const string FormationRoute = Base + "/formation";

            public const string Create = FormationRoute + "/create";

            public const string Get = FormationRoute + "/get/{id}";

            public const string GetAll = FormationRoute + "/getAll";

            public const string GetAllPublished = FormationRoute + "/getAll/published";
        }

        public static class FormationRequestRoutes
        {
            public const string FormationRequestRoute = Base + "/formationRequest";

            public const string Create = FormationRequestRoute + "/create";

            public const string Get = FormationRequestRoute + "/get/{studantId}/{formationId}";
        }

        public static class FormationEventRoutes
        {
            public const string FormationEventRoute = Base + "/formationEvent";

            public const string Create = FormationEventRoute + "/create";

            public const string Get = FormationEventRoute + "/get/{id}";

            public const string GetAll = FormationEventRoute + "/getAll";
        }

        public static class LessonRoutes
        {
            public const string LessonRoute = Base + "/lesson";

            public const string Create = LessonRoute + "/create";

            public const string GetAll = LessonRoute + "/getAll";

            public const string GetAllByStudant = LessonRoute + "/getAll/studant/{studantId}";

            public const string GetAllByTeacherPlace = LessonRoute + "/getAll/teacherPlace/{teacherPlaceId}";

            public const string Get = LessonRoute + "/get/{id}";

            public const string GetByPost = LessonRoute + "/get/post/{postId}";
        }

        public static class ManagerRoutes
        {
            public const string ManagerRoute = Base + "/manager";

            public const string Create = ManagerRoute + "/create";

            public const string Get = ManagerRoute + "/get/{id}";

            public const string GetByUser = ManagerRoute + "/getByUser/{userId}";
        }        
        public static class ModuleRoutes
        {
            public const string ModuleRoute = Base + "/module";

            public const string Create = ModuleRoute + "/create";

            public const string Get = ModuleRoute + "/get/{id}";

            public const string GetAll = ModuleRoute + "/getAll";
        }
        public static class OrganRoutes
        {
            public const string OrganRoute = Base + "/organ";

            public const string Create = OrganRoute + "/create";

            public const string GetAll = OrganRoute + "/getAll";

            public const string Get = OrganRoute + "/get/{id}";
        }
        public static class PostRoutes
        {
            public const string PostRoute = Base + "/post";

            public const string Create = PostRoute + "/create";

            public const string GetAll = PostRoute + "/getAll";

            public const string Update = PostRoute + "/update/{id}";

            public const string Get = PostRoute + "/get/{id}";
        }
        public static class QuestionRoutes
        {
            public const string QuestionRoute = Base + "/question";

            public const string Create = QuestionRoute + "/create";

            public const string GetAll = QuestionRoute + "/getAll";

            public const string Patch = QuestionRoute + "/patch/{id}";

            public const string GetAllByStudant = QuestionRoute + "/getAll/studant/{studantId}";

            public const string GetAllByTeacherPlace = QuestionRoute + "/getAll/teacherPlace/{teacherPlaceId}";

            public const string Update = QuestionRoute + "/update/{id}";

            public const string Get = QuestionRoute + "/get/{id}";

            public const string GetByPost = QuestionRoute + "/get/post/{postId}";
        }

        public static class SchoolRoutes
        {
            public const string SchoolRoute = Base + "/school";

            public const string Get = SchoolRoute + "/get/{id}";
            public const string Create = SchoolRoute + "/create";
            public const string GetByUser = SchoolRoute + "/getByUser/{userId}";
        }

        public static class StudantRoutes
        {
            public const string StudantRoute = Base + "/studant";

            public const string Create = StudantRoute + "/create";

            public const string GetAll = StudantRoute + "/getAll";

            public const string Update = StudantRoute + "/update/{id}";

            public const string Get = StudantRoute + "/get/{id}";

            public const string GetResponsable = StudantRoute + "/get/responsable/{id}";
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
        public static class ImagesRoutes
        {
            public const string ImageRoute = Base + "/image";

            public const string UploadLessonProfile = ImageRoute + "/upload/lesson/profile";
            public const string UploadTopicImage = ImageRoute + "/upload/topic";

            public const string UploadModuleImage = ImageRoute + "/upload/module";
        }

        public static class VideoRoutes
        {
            public const string VideoRoute = Base + "/video";

            public const string Create = VideoRoute + "/create";

            public const string Upload = VideoRoute + "/upload";

            public const string GetAll = VideoRoute + "/getAll";

            public const string Get = VideoRoute + "/get/{id}";

            public const string VideoWatch = VideoRoute + "/upload/watch/{connectionId}";
        }
    }
}
