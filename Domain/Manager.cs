using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alumni.Domain
{
    public class Manager
    {
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string SchoolId { get; set; }

        [Required]
        public string Situation { get; set; }

        [Required]
        public DateTime DateSituation { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(SchoolId))]
        public School School { get; set; }
    }
}
