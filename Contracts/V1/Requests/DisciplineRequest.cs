using System.ComponentModel.DataAnnotations;


namespace alumni.Contracts.V1.Requests
{
    public class DisciplineRequest
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string BadgeInformationId { get; set; }
    }
}
