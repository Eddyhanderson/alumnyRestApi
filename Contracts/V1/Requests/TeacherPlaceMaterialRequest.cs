using System.ComponentModel.DataAnnotations;


namespace alumni.Contracts.V1.Requests
{
    public class TeacherPlaceMaterialRequest
    {
        public string Id { get; set; }

        public string PostId { get; set; }

        [Required]
        public string TeacherPlaceId { get; set; }

        [Required]
        public string MaterialPath { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
