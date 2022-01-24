using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string RefreshToken { get; set; }

        public string Token { get; set; }

        [Required]
        public string LoginProvider { get; set; }

        [Required]
        public string LoginKey { get; set; }

        [Required]
        public string GrantType { get; set; }
    }
}
