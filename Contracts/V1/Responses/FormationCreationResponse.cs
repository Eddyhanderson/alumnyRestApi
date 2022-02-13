using System;

namespace alumni.Contracts.V1.Responses
{
    public class FormationCreationResponse
    {
        public string Id { get; set; }
        public string Theme { get; set; }
        public int Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SchoolId { get; set; }

        public string Situation { get; set; }

        public DateTime DateSituation { get; set; }
        public DateTime DateCreation { get; set; }
    }
}