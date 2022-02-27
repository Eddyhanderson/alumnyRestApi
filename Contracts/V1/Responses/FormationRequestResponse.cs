using System;

namespace alumni.Contracts.V1.Responses
{
    public class FormationRequestResponse
    {
        public string Id { get; set; }
        public string FormationId;
        public string StudantId;
        public string StudantMessage;
        public string TeacherMessage;
        public string State;
        public DateTime CreateAt { get; set; }
        public DateTime StateDate { get; set; }
        public string FormationTheme { get; set; }
        public string FormationSchoolPicture { get; set; }

        public string FormationSchoolAcronym { get; set; }
        public string FormationSchoolName { get; set; }
        public decimal FormationPrice { get; set; }
        public DateTime FormationStart { get; set; }
        public string StudantFisrtName { get; set; }
        public string StudantLastName { get; set; }
        public string StudantPicture { get; set; }
    }
}