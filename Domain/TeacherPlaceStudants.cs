using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class TeacherPlaceStudants
    {
        [Required]
        public string TeacherPlaceId { get; set; }

        [Required]
        public string StudantId { get; set; }

        [Required]
        public DateTime RegisteredAt { get; set; }

        [Required]
        public int Months { get; set; }

        [Required]
        public string Situation { get; set; }

        [Required]
        public DateTime DateSituation { get; set; }

        [ForeignKey(nameof(TeacherPlaceId))]
        public TeacherPlace TeacherPlace { get; set; }

        [ForeignKey(nameof(StudantId))]
        public Studant Studant { get; set; }
    }
}
