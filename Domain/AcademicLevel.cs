using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class AcademicLevel
    {
        [Key]
        public string Id { get; set; }

        [Required]        
        public string Name { get; set; }
    }
}
