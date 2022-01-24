using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class Studant
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string StudantCode { get; set; }
        
        public string AcademyId { get; set; }

        [Required]
        public string AcademicLevelId { get; set; }

        public string CourseId { get; set; }

        [ForeignKey(nameof(AcademyId))]
        public Academy Academy { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }

        [ForeignKey(nameof(AcademicLevelId))]
        public AcademicLevel AcademicLevel { get; set; }
    }
}
