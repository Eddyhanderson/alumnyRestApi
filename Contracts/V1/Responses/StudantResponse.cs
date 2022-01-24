using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class StudantResponse
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string UserId { get; set; }

        public string PictureProfilePath { get; set; }

        public string AcademicLevelId { get; set; }

        public string AcademicLevelName { get; set; }

        public string AcademyId { get; set; }

        public string AcademyName { get; set; }

        public string CourseId { get; set; }

        public string CourseName { get; set; }

        public string StudantCode { get; set; }
    }
}
