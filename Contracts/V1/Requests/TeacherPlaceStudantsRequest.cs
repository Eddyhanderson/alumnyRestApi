using Alumni.Helpers;
using System;
using System.ComponentModel.DataAnnotations;


namespace alumni.Contracts.V1.Requests
{
    public class TeacherPlaceStudantsRequest
    {
        [Required]
        public string TeacherPlaceId { get; set; }

        [Required]
        public string StudantId { get; set; }

        [Required]
        public TeacherPlaceRegistrationState Situation { get; set; }

        [Required]
        public int Months { get; set; }
    }
}
