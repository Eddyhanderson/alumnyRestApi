using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class RegisterInCourseRequest
    {
        // is part of a composite key
        public string CourseId { get; set; }

        // is part of a composite key
        public string StudantId { get; set; }

        // is part of a composite key
        [Required]
        public string Situation { get; set; }
        
        public string AdminId { get; set; }

        public bool Accepted { get; set; }
        
        public DateTime RequestAt { get; set; }

        public DateTime AcceptAt { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }

        [ForeignKey(nameof(StudantId))]
        public Studant Studant { get; set; }

        [ForeignKey(nameof(AdminId))]
        public Admin Admin { get; set; }
    }
}
