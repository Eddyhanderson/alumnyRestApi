using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests.Queries
{
    public class FormationEventQuery
    {
        public string FormationId { get; set; }

        public string Situation { get; set; }

        public int Category { get; set; }
    }
}
