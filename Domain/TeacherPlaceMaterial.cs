using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace alumni.Domain
{
    public class TeacherPlaceMaterial
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string PostId { get; set; }

        [Required]
        public string TeacherPlaceId { get; set; }

        [Required]
        public string MaterialPath { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }

        [ForeignKey(nameof(TeacherPlaceId))]
        public TeacherPlace TeacherPlace { get; set; }
    }
}
