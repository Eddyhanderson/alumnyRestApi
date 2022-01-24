using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class TeacherPlaceMessageResponse
    {
        public string Id { get; set; }

        public string PostId { get; set; }

        public string TeacherPlaceId { get; set; }

        public string Message { get; set; }
    }
}
