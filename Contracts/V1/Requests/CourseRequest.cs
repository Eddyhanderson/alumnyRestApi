using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class CourseRequest
    {        
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string BadgeInformationId { get; set; }
    }
}
