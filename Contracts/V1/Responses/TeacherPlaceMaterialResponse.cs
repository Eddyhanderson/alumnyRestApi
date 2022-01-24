using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class TeacherPlaceMaterialResponse
    {
        public string Id { get; set; }

        public string PostId { get; set; }

        public string TeacherPlaceId { get; set; }

        public string MaterialPath { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
