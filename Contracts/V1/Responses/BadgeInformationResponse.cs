using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class BadgeInformationResponse
    {
        public string Id { get; set; }

        public UserResponse User { get; set; }

        public DateTime CreatAt { get; set; }

        public string Situation { get; set; }

        public DateTime DateSituation { get; set; }

        public string ProfilePhotoPath { get; set; }
    }
}
