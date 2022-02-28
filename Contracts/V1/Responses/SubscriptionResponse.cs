using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class SubscriptionResponse
    {
        public string Id { get; set; }

        public string StudantId { get; set; }

        public string FormationEventId { get; set; }

        public string Situation { get; set; }
    }
}
