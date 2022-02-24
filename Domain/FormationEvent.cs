using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alumni.Domain
{
    public class FormationEvent
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public int StudantLimit { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Situation { get; set; }

        [Required]
        public string FormationId { get; set; }

        [ForeignKey(nameof(FormationId))]
        public Formation Formation { get; set; }
    }
}