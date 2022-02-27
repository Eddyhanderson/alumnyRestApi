using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alumni.Domain
{
    public class FormationRequest
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string FormationId { get; set; }

        [Required]
        public string StudantId { get; set; }

        public string StudantMessage { get; set; }
        
        public string TeacherMessage { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }

        [Required]
        public DateTime StateDate { get; set; }

        [ForeignKey(nameof(FormationId))]
        public Formation Formation { get; set; }

        [ForeignKey(nameof(StudantId))]
        public Studant Studant { get; set; }

        [Required]
        public string Situation { get; set; }
    }
}