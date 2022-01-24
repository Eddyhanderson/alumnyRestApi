using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class PostRequest
    {
        public string Id { get; set; }

        [Required]
        public string PostType { get; set; }

        public string Situation { get; set; }
    }
}
