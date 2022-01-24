using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class ArticleResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Delta { get; set; }

        public bool Draft { get; set; }

        public DateTime LastChange { get; set; }

        public string TeacherId { get; set; }

        public string Situation { get; set; }
    }
}
