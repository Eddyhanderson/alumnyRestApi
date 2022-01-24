using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests.Queries
{
    public class CourseQuery
    {
        public string SchoolId { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
