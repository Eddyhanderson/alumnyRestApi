using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class LessonResponse
    {
        public string Id { get; set; }

        public string ArticleId { get; set; }

        public string SchoolId { get; set; }
        
        public string PostId { get; set; }

        public string ModuleId { get; set; }        

        public string Picture { get; set; }

        public string ManifestPath { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }
        
        public int Sequence { get; set; }

        public int Views { get; set; }

        public string LessonType { get; set; }

        public string Duration { get; set; }

        public DateTime Date { get; set; }
    }
}
