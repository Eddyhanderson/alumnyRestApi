using Alumni.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests.Queries
{
    public class TeacherSchoolQuery
    {
        public string TeacherId { get; set; }

        public string SchoolId { get; set; }

        [RegularExpression(Constants.RegexExpressions.SituationRegex)]
        public string Situation { get; set; }
    }
}
