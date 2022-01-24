using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests.Queries
{
    public class TeacherPlaceQuery
    {
        public string TeacherId { get; set; }
        public string SchoolId { get; set; }
        public string CourseId { get; set; }
        public string StudantId { get; set; }
        public string Opened { get; set; }
    }
}
