using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class ManagerRequest
    {
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string SchoolId { get; set; }        
    }
}
