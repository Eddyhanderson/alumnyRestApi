using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests.Queries
{
    public class FormationRequestQuery
    {
        public string StudantId { get; set; }

        public bool IsResponsable { get; set; }

        public bool IsSchool { get; set; }

        public bool IsManager { get; set; }
    }
}
