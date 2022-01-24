﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class SchoolCoursesRequest
    {
        [Required]
        public string SchoolId { get; set; }

        [Required]
        public string CourseId { get; set; }

        public string Situation { get; set; }
    }
}