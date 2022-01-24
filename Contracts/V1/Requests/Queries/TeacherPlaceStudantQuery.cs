using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests.Queries
{
    public class TeacherPlaceStudantQuery
    {
        public string TeacherPlaceId { get; set; }

        public string StudantId { get; set; }

        public string Situation { get; set; }
    }
}
