using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class SchoolCourses
    {
        [Required]
        public string CourseId { get; set; }

        [Required]
        public string SchoolId { get; set; }

        [Required]
        public string Situation { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }
        
        [ForeignKey(nameof(SchoolId))]
        public School School { get; set; }
    }
}
