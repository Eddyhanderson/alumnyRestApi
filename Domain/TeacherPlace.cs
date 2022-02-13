using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class TeacherPlace
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string TeacherId { get; set; }

        [Required]
        public string DisciplineId { get; set; }

        [Required]
        public string CourseId { get; set; }

        [Required]
        public string SchoolId { get; set; }

        [Required]
        public string Situation { get; set; }

        [Required]
        public string TeacherPlaceCode { get; set; }

        [Required]
        public string Name { get; set; }

        public string ProfilePhotoPath { get; set; }

        [Required]
        public string Description { get; set; }

        public bool Opened { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public Teacher Teacher { get; set; }

        [ForeignKey(nameof(DisciplineId))]
        public Discipline Discipline { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }
        
        [ForeignKey(nameof(SchoolId))]
        public SchoolDeprecated School { get; set; }
    }
}
