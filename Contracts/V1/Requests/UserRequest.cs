using Alumni.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class UserRequest
    {
        [RegularExpression(Constants.RegexExpressions.EmailRegex)]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [RegularExpression(Constants.RegexExpressions.NomenclatureRegex)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(Constants.RegexExpressions.NomenclatureRegex)]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [RegularExpression(Constants.RegexExpressions.RoleRegex)]
        public string Role { get; set; }

        [Required]
        [RegularExpression(Constants.RegexExpressions.GenreRegex)]
        public string Genre { get; set; }

        [Required]
        public DateTime Birth { get; set; }
    }
}
