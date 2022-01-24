using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class Question
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string LessonId { get; set; }

        [Required]
        public string PostId { get; set; }

        [Required]
        public string StudantId { get; set; }

        public string Content { get; set; }

        public string Subject { get; set; }

        [Required]
        public string Situation { get; set; }

        [ForeignKey(nameof(StudantId))]
        public Studant Studant { get; set; }

        [ForeignKey(nameof(LessonId))]
        public Lesson Lesson { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }
    }
}
