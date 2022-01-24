using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class School
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string BadgeInformationId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ShortName { get; set; }
        
        public string Nif { get; set; }

        public string Address { get; set; }

        public bool Entrusted { get; set; }

        [ForeignKey(nameof(BadgeInformationId))]
        public BadgeInformation BadgeInformation { get; set; }
    }
}
