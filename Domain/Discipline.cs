using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alumni.Domain
{
    public class Discipline
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string BadgeInformationId { get; set; }

        [ForeignKey(nameof(BadgeInformationId))]
        public BadgeInformation BadgeInformation { get; set; }
    }
}
