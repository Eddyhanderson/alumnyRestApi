using alumni.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class StudantRequest
    {
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public string AcademyId { get; set; }

        public string CourseId { get; set; }

        [Required]
        public string AcademicLevelId { get; set; }
    }
}
