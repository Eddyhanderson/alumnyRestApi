using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class RegisterInSchoolRequest
    {
        // is part of a composite key
        public string SchoolId { get; set; }

        // is part of a composite key
        public string StudantId { get; set; }

        // is part of a composite key
        [Required]
        public string Situation { get; set; }

        public string AdminId { get; set; }

        public bool Accepted { get; set; }

        public DateTime RequestAt { get; set; }

        public DateTime AcceptAt { get; set; }

        public DateTime DateSituation { get; set; }

        [ForeignKey(nameof(SchoolId))]
        public School School { get; set; }

        [ForeignKey(nameof(StudantId))]
        public Studant Studant { get; set; }

        [ForeignKey(nameof(AdminId))]
        public Admin Admin { get; set; }
    }
}
