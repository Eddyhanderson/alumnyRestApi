using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alumni.Helpers
{
    public static class Constants
    {
        public static class UserContansts
        {
            public const string StudantRole = "Studant";

            public const string TeacherRole = "Teacher";

            public const string AdminRole = "Manager";

            public const string IdClaimType = "Id";

            public const string GrantPassword = "Password";

            public const string GrantRefresh = "RefreshToken";

            public const string AboutUsertDefault = "Olá, bora estudar juntos? Sempre fomos mais fortes juntos.";

        }

        public static class SituationsObjects
        {
            public const string NormalSituation = "Normal";

            public const string DeletedSituation = "Deleted";

            public const string PendingSituation = "Pending";

            public const string Unsubscribed= "Unsubscribed";
        }

        public static class RegexExpressions
        {
            public const string NomenclatureRegex = @"^[a-zA-Z]+$";

            public const string SimpleInformationRegex = @"^[a-zA-Z0-9 ]*{1,15}$";

            public const string StandardInformationRegex = @"^[a-zA-Z]*{1,15}$";

            public const string EmailRegex = @"^[a-zA-Z0-9_]+@[a-zA-Z]+[.][a-zA-Z]+$";

            public const string RoleRegex = @"(Studant|Teacher|Manager)";

            public const string SituationRegex = @"(Normal|Pending|Deleted|Unsubscribed)";

            public const string GenreRegex = @"(M|F|O)";

            public const string LessonTypeRegex = @"(Video|Article)";
        }

        public static class BadgeInformationTypes
        {
            public const string CompanyBadge = "Company";

            public const string FunctionCompanyBadge = "FunctionCompany";

            public const string AcademiaBadge = "Academia";

            public const string AcademicDegreeBadge = "AcademicDegree";

            public const string CourseBadge = "Course";

            public const string DisciplineBadge = "Discipline";
        }
        
        public static class PathFiles
        {
            public const string MediaPath = "Media";

            public const string RequestPath = "/MediaFiles";
        }

        public static class ServerMessages
        {
            public const string TokenNotCreated = "O sistema não conseguiu gerar as suas credenciais";

            public const string EmailAlreadyExists = "O email já existe. Insira um email diferente.";

            public const string LoginAuthFail = "Palavra pase ou email inválido";
        }

        public static class ModelMessages
        {
            public const string FailModelCreated = "O Sistema não consegue guardar a informação. Tente novamente.";

            public const string FailModelPersist = "The system cannot save this model";
        }
    }     

    public enum LessonTypes
    {
        Video,
        Article
    }
  
    public enum PostsTypes
    {
        Question,
        QuestionAnswer,
        Article,
        Lesson,
        Comment,
        TeacherMessage,
        TeacherMaterial,
        Topic
    }

    public enum QuestionSituations
    {
        Solved,
        Closed,
        Opened,
        Waiting,
        Analyzing,
        All
    }

    public enum TeacherPlaceRegistrationState
    {
        Registered,
        UnRegistered
    }
}
