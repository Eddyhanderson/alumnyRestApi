using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alumni.Domain
{
    public class Module
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string FormationId { get; set; }

        [ForeignKey(nameof(FormationId))]
        public Formation Formation { get; set; }

        public string Picture { get; set; }

        [Required]
        public int Sequence { get; set; }

        [Required]
        public string Situation { get; set; }

        [Required]
        public DateTime DateSituation { get; set; }

        public DateTime DateCreation { get; set; }
    }

}