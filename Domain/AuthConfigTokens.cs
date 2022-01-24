using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class AuthConfigTokens
    {
        [Key]
        public string RefreshToken { get; set; }

        [Required]
        public string Jwti { get; set; }

        [Required]
        public string TokenValue { get; set; }

        [Required]
        public string UserId { get; set; }

        public bool Used { get; set; }

        public bool Invalidated { get; set; }

        [Required]
        public DateTime ExpireAt { get; set; }

        [Required]
        public DateTime CreationAt { get; set; }


    }
}
