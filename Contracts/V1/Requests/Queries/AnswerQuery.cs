using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests.Queries
{
    public class AnswerQuery
    {
        [Required]
        public string QuestionId { get; set; }
    }
}
