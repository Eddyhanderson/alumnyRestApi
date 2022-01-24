using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class SchoolCoursesResponse
    {
        public SchoolResponse School { get; set; }

        public CourseResponse Course { get; set; }

        public string Situation { get; set; }
    }
}
