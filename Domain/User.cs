using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class User : IdentityUser
    {
        public string Picture { get; set; }

        public string Genre { get; set; }

        public string Birth { get; set; }

        [Required]
        public string Situation { get; set; }

        [Required]
        public DateTime DateSituation { get; set; }
    }
}
