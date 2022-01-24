using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class Topic
    {
        [Key]
        public string Id { get; set; }

        public string PhotoProfile { get; set; }

        public string Description { get; set; }

        public string DisciplineTopicId { get; set; }

        public string TeacherPlaceId { get; set; }

        public string PostId { get; set; }

        [ForeignKey(nameof(DisciplineTopicId))]
        public DisciplineTopic DisciplineTopic { get; set; }

        [ForeignKey(nameof(TeacherPlaceId))]
        public TeacherPlace TeacherPlace { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }
    }
}
