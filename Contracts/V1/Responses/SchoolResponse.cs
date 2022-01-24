using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class SchoolResponse
    {
        public string Id { get; set; }

        public string BadgeInformationId { get; set; }

        public string ProfilePhotoPath { get; set; }

        public string ShortName { get; set; }

        public string Name { get; set; }

        public string Nif { get; set; }

        public string Address { get; set; }

        public bool Entrusted { get; set; }
    }
}
