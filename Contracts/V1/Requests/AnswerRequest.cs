using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class AnswerRequest
    {
        public string Id { get; set; }

        [Required]
        public string QuestionId { get; set; }

        [Required]
        public string Content { get; set; }

        public string PostId { get; set; }
    }
}
