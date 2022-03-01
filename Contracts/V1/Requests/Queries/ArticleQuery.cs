using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests.Queries
{
    public class ArticleQuery
    {
        public bool Draft { get; set; }

        public string ModuleId { get; set; }

        public string FormationId { get; set; }

        public string SchoolId { get; set; }
    }
}
