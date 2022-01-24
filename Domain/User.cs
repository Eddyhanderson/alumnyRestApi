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
        public string PictureProfilePath { get; set; }

        [Required]
        public string Situation { get; set; }

        public string AboutUser { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public DateTime Birth { get; set; }

        [Required]
        public DateTime CreationAt { get; set; }
    }
}
