using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class AcademyResponse
    {        
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public BadgeInformationResponse BadgeInformation { get; set; }
        
    }
}
