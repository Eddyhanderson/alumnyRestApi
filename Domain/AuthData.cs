using Alumni.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class AuthData
    {
        [Required]
        public string Password { get; set; }

        [Required]
        [RegularExpression(Constants.RegexExpressions.RoleRegex)]
        public string Role { get; set; }
    }
}
