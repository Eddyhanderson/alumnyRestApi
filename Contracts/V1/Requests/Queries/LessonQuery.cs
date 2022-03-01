using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests.Queries
{
    public class LessonQuery
    {
        public string ModuleId { get; set; }
        public string SchoolId { get; set; }

        public string FormationId {get;set;}
    }
}
