using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class TeacherSchools
    {
        [Required]
        public string TeacherId { get; set; }

        [Required]
        public string SchoolId { get; set; }
        
        [Required]
        public string Situation { get; set; }

        [Required]
        public DateTime DateSituation { get; set; }

        [Required]
        public DateTime CreationAt { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public Teacher Teacher { get; set; }

        [ForeignKey(nameof(SchoolId))]
        public School School { get; set; }
    }
}
