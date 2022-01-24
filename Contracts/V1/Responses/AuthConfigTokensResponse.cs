using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class AuthConfigTokensResponse
    {
        [Key]
        public string RefreshToken { get; set; }

        [Required]
        public string TokenValue { get; set; }
    }
}
