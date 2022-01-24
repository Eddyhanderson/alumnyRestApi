using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests.Queries
{
    public class DisciplineTopicQuery
    {
        public string TeacherPlaceId { get; set; }
        public string TeacherId { get; set; }
        public string SchoolId { get; set; }
    }
}
