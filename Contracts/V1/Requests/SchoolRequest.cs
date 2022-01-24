using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class SchoolRequest
    {        
        public string Id { get; set; }
        
        public string BadgeInformationId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ShortName { get; set; }

        public string Nif { get; set; }

        public string Address { get; set; }

        public bool Entrusted { get; set; }
    }
}
