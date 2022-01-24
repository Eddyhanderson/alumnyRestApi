using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class AnswerResponse
    {
        public string Id { get; set; }

        public string QuestionId { get; set; }

        public string PostId { get; set; }

        public string CommentableId { get; set; }

        public string UserId { get; set; }

        public string Content { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string UserPhoto { get; set; }

        public string UserCourse { get; set; }

        public string UserAcademy { get; set; }

        public string UserAcademicLevel { get; set; }

        public string UserRole { get; set; }

        public IEnumerable<CommentResponse> Comments { get; set; }

        public DateTime CreateAt { get; set; }

    }
}
