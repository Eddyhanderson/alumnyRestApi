using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Alumni.Helpers;

namespace alumni.Contracts.V1.Requests
{
    public class CreateManagerRequest
    {

        [Required]
        [RegularExpression(Constants.RegexExpressions.NomenclatureRegex)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(Constants.RegexExpressions.NomenclatureRegex)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(Constants.RegexExpressions.EmailRegex)]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        public string Picture { get; set; }
    }
}
