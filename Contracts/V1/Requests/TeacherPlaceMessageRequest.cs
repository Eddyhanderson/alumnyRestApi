using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class TeacherPlaceMessageRequest
    {
        public string Id { get; set; }

        public string PostId { get; set; }

        [Required]
        public string TeacherPlaceId { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
