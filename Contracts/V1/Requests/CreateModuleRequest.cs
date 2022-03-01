using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Alumni.Helpers;

namespace alumni.Contracts.V1.Requests
{
    public class CreateModuleRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public string Picture { get; set; }

        [Required]
        public string FormationId { get; set; }
    }
}
