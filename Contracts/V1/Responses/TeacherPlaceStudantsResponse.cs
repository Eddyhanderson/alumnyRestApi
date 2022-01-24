using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class TeacherPlaceStudantsResponse
    {
        public string TeacherPlaceId { get; set; }

        public string StudantId { get; set; }

        public DateTime RegisteredAt { get; set; }

        public int Months { get; set; }

        public string Situation { get; set; }

        public DateTime DateSituation { get; set; }

        public TeacherPlaceResponse TeacherPlace { get; set; }

        public StudantResponse Studant { get; set; }
    }
}
