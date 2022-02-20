using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class Lesson
    {
        [Key]
        [Required]
        public string Id { get; set; }

        public string Picture { get; set; }

        [Required]
        public string ModuleId { get; set; }

        public string VideoId { get; set; }

        public string ArticleId { get; set; }

        [Required]
        public string PostId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Sequence { get; set; }

        public int Views { get; set; }    
        
        [Required]
        public string LessonType { get; set; }

        [ForeignKey(nameof(VideoId))]
        public Video Video { get; set; }

        [ForeignKey(nameof(ArticleId))]
        public Article Article { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }

        [ForeignKey(nameof(ModuleId))]
        public Module Module { get; set; }
    }
}
