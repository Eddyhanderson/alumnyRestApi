using Alumni.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests.Queries
{
    public class QuestionQuery
    {
        public string TeacherPlaceId { get; set; }

        public string LessonId { get; set; }

        public string StudantId { get; set; }

        public string TeacherId { get; set; }

        public QuestionSituations Situation { get; set; }
    }
}
