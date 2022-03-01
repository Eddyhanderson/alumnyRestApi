using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class ArticleRequest
    {        
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Delta { get; set; }

        [Required]
        public bool Draft { get; set; }                

        [Required]
        public string ModuleId { get; set; }

        public DateTime LastChange { get; set; }

        public string Situation { get; set; }
    }
}
