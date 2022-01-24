using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class Course
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string BadgeInformationId { get; set; }

        [ForeignKey(nameof(BadgeInformationId))]
        public BadgeInformation BadgeInformation { get; set; }
    }
}
