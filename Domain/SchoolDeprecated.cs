using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class SchoolDeprecated
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string SchoolIdentityId { get; set; }
    }
}
