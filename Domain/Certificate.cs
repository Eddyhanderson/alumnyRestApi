using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alumni.Domain
{
    public class Certificate
    {
        [Key]
        public string Id { get; set; }
        public string AssessmentMethod { get; set; }

        public string AssessmentScore { get; set; }

        public string QualitativeResult { get; set; }

        public string MaxScore { get; set; }

        public string PathCertificate { get; set; }

        public string Observation { get; set; }

        [Required]
        public string SubscriptionId { get; set; }

        [Required]
        public DateTime EmitedAt { get; set; }

        [Required]
        public string AssignmentSchool { get; set; }

        [ForeignKey(nameof(SubscriptionId))]
        public Subscription Subscription { get; set; }
    }

}