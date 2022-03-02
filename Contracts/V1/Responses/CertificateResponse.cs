namespace alumni.Contracts.V1.Responses
{
    public class CertificateResponse
    {
        public string Id { get; set; }
        public string AssessmentMethod { get; set; }

        public string AssessmentScore { get; set; }

        public string QualitativeResult { get; set; }

        public string MaxScore { get; set; }

        public string PathCertificate { get; set; }

        public string Observation { get; set; }

        public string SubscriptionId { get; set; }

        public string AssignmentSchool { get; set; }
    }
}