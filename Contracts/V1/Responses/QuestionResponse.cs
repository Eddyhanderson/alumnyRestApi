using alumni.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class QuestionResponse
    {       
        public string StudantId { get; set; }

        public string PostId { get; set; }

        public string Content { get; set; }

        public string Subject { get; set; }

        [Required]
        public string Id { get; set; }

        public string CommentableId { get; set; }

        public string LessonId { get; set; }

        public string Situation { get; set; }

        public string LessonBackgroundPhotoPath { get; set; }

        public string LessonTitle { get; set; }

        public int LessonSequence { get; set; }

        public string LessonType { get; set; }

        public string StudantFirstName { get; set; }

        public string StudantLastName { get; set; }

        public string StudantPhoto { get; set; }

        public DateTime CreateAt { get; set; }

        public int StudantAnswerQnt { get; set; }

        public int TeacherAnswerQnt { get; set; }

        public int CommentsQnt { get; set; }
    }
}
