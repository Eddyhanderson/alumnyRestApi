using Alumni.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class CreateStudantRequest
    {   
        [Required]
        public string OrganId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string StudantCode { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        public string Picture { get; set; }

        [Required]
        public DateTime Birth { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public bool Resposanble { get; set; }

    }
}
