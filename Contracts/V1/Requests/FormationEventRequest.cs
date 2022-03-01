using System;

namespace alumni.Contracts.V1.Requests
{
    public class FormationEventRequest
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int StudantLimit { get; set; }

        public string FormationId { get; set; }
    }
}