using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class TeacherSchoolsRequest
    {
        [Required]
        public string TeacherId { get; set; }

        [Required]
        public string SchoolId { get; set; }

        public string Situation { get; set; }
    }
}
