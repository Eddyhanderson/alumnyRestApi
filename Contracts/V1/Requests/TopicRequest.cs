using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class TopicRequest
    {        
        public string PhotoProfile { get; set; }

        public string Description { get; set; }

        [Required]
        public string DisciplineTopicId { get; set; }

        [Required]
        public string TeacherPlaceId { get; set; }
    }
}
