using System.ComponentModel.DataAnnotations;

namespace alumni.Contracts.V1.Requests
{
    public class TeacherRequest
    {        
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }
        
        public string TeacherCode { get; set; }

        public string AcademyId { get; set; }

        public string CourseId { get; set; }

        [Required]
        public string AcademicLevelId { get; set; }        
    }
}
