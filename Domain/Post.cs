using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class Post
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string PostType { get; set; }

        [Required]
        public string CommentableId { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }

        [Required]
        public string Situation { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(CommentableId))]
        public Commentable Commentable { get; set; }

    }
}
