using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class StudantResponse
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserId { get; set; }

        public string Picture { get; set; }

        public string StudantCode { get; set; }

        public bool Responsable { get; set; }

        public string OrganId { get; set; }

        public string OrganName { get; set; }
    }
}
