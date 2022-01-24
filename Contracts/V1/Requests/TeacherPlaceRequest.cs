using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class TeacherPlaceRequest
    {
        public string Id { get; set; }

        [Required]
        public string SchoolId { get; set; }

        [Required]
        public string TeacherId { get; set; }

        [Required]
        public string DisciplineId { get; set; }

        [Required]
        public string CourseId { get; set; }

        [Required]
        public string Name { get; set; }

        public string ProfilePhotoPath { get; set; }     
        
        [Required]
        public bool Opened { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; }
    }
}
