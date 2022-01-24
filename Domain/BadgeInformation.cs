using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class BadgeInformation
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime CreatAt { get; set; }

        [Required]
        public string Situation { get; set; }
        
        [Required]
        public DateTime DateSituation { get; set; }

        public string ProfilePhotoPath { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
