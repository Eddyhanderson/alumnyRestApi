using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests.Queries
{
    public class SchoolQuery
    {
        public string TeacherId { get; set; }

        public bool Subscribed { get; set; }
    }
}
