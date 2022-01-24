using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class TeacherSchoolsResponse
    {        
        public TeacherResponse Teacher { get; set; }

        public SchoolResponse School { get; set; }
        
        public string Situation { get; set; }
       
        public DateTime DateSituation { get; set; }
        
        public DateTime CreationAt { get; set; }
    }
}
