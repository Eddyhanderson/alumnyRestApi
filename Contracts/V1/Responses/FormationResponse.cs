using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Alumni.Helpers;

namespace alumni.Contracts.V1.Responses
{
    public class FormationResponse
    {
        public string Id { get; set; }

        public string Theme { get; set; }

        public int Category { get; set; }

        public bool Published { get; set; }

        public string Description { get; set; }

        public string Picture { get; set; }

        public decimal Price { get; set; }

        public string SchoolId { get; set; }

        public string SchoolPicture { get; set; }

        public string SchoolAcronym { get; set; }

        public string SchoolName { get; set; }

        public string Situation { get; set; }

        public DateTime DateSituation { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int ModulesCount { get; set; }

        public int LessonCount { get; set; }

        public int SubscriptionCount { get; set; }

        public int StudantLimit { get; set; }

        public string State { get; set; }
    }
}
