using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests.Queries
{
    public class SubscriptionQuery
    {
        public List<string> State { get; set; }
        public string StudantId { get; set; }

        public string SchoolId { get; set; }
    }
}
