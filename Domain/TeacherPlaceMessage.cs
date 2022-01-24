using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class TeacherPlaceMessage
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string PostId { get; set; }

        [Required]
        public string TeacherPlaceId { get; set; }

        [Required]
        public string Message { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }
    }
}
