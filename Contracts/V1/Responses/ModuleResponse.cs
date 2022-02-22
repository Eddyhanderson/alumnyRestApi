using System;

namespace alumni.Contracts.V1.Responses
{
    public class ModuleResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Picture { get; set; }

        public string FormationId { get; set; }

        public int Sequence { get; set; }
    }
}