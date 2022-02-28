namespace alumni.Contracts.V1.Requests
{
    public class FormationRequestRequest
    {
        public string Id { get; set; }
        public string FormationId { get; set; }
        public string StudantId { get; set; }
        public string StudantMessage { get; set; }
        public string TeacherMessage { get; set; }
        public string Status { get; set; }
    }
}