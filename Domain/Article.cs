using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alumni.Domain
{
    public class Article
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Situation { get; set; }

        [Required]
        public string Delta { get; set; }

        [Required]
        public bool Draft { get; set; }

        [Required]
        public DateTime LastChange { get; set; }

        [Required]
        public string ModuleId { get; set; }

        [ForeignKey(nameof(ModuleId))]
        public Module Module { get; set; }
    }
}
