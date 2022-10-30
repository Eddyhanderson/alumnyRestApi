using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class SubscriptionResponse
    {
        public string Id { get; set; }
        public string StudantId { get; set; }
        public string StudantName { get; set; }
        public string StudantPicture { get; set; }
        public string FormationEventId { get; set; }
        public string FormationTheme { get; set; }
        public string FormationId { get; set; }
        public string FormationSchoolName { get; set; }
        public DateTime FormationStart { get; set; }
        public DateTime FormationEnd { get; set; }
        public string FormationSchoolPicture { get; set; }
        public string State { get; set; }
        public string CertificateId { get; set; }
        public string Situation { get; set; }
    }
}
