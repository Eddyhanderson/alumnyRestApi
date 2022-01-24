using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class Academy
    {
        [Key]
        public string Id { get; set; }

        public string BadgeInformationId { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey(nameof(BadgeInformationId))]
        public BadgeInformation BadgeInformation { get; set; }
    }
}
