using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class Studant
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string OrganId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public bool Responsable { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string StudantCode { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(OrganId))]
        public Organ Organ { get; set; }

    }
}
