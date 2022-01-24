using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class Notification
    {
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public string Messages { get; set; }

        [Required]
        public string Situation { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
