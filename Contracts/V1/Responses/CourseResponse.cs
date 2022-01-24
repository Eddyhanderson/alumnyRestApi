using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class CourseResponse
    {
        public string Id { get; set; }

        public string BadgeInformationId { get; set; }

        [Required]
        public string Name { get; set; }

        public string ProfilePhotoPath { get; set; }
    }
}
