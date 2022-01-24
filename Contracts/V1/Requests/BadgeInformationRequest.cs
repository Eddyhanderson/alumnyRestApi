using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class BadgeInformationRequest
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public DateTime CreatAt { get; set; }

        public string Situation { get; set; }

        public DateTime DateSituation { get; set; }

        public string ProfilePhotoPath { get; set; }
    }
}
