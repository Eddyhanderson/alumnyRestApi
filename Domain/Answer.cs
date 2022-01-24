using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class Answer
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string QuestionId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; }
    }
}
