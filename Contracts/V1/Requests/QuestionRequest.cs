using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class QuestionRequest
    {
        public string Id { get; set; }

        [Required]
        public string LessonId { get; set; }

        public string PostId { get; set; }

        public string Content { get; set; }

        public string Subject { get; set; }

        public string StudantId { get; set; }
    }
}
