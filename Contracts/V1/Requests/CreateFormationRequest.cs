using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Alumni.Helpers;

namespace alumni.Contracts.V1.Requests
{
    public class CreateFormationRequest
    {
        [Required]
        public string Theme { get; set; }

        [Required]
        public int Category { get; set; }

        public string Picture { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string SchoolId { get; set; }
    }
}
