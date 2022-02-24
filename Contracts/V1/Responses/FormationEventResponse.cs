using System;

namespace alumni.Contracts.V1.Responses
{
    public class FormationEventResponse
    {
        public string Id { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int StudantLimit { get; set; }

        public string Situation { get; set; }

        public string State { get; set; }

        public string FormationId { get; set; }
    }
}