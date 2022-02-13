using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class SchoolResponse
    {
        public string Id { get; set; }

        public string SchoolCode { get; set; }

        public string UserId { get; set; }
        public string Name { get; set; }

        public string Acronym { get; set; }

        public string Nif { get; set; }

        public string Adress { get; set; }

        public UserResponse User { get; set; }
    }
}
