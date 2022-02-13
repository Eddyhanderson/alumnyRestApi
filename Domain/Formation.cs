using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alumni.Domain
{

    public class Formation
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Theme { get; set; }

        [Required]
        public int Category { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string SchoolId { get; set; }

        [ForeignKey(nameof(SchoolId))]
        public School School { get; set; }

        [Required]
        public string Situation { get; set; }

        [Required]
        public DateTime DateSituation { get; set; }

        [Required]
        public DateTime DateCreation { get; set; }
    }

}