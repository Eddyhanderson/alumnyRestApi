using System;
using System.ComponentModel.DataAnnotations;

namespace alumni.Contracts.V1.Responses
{
    public class ManagerResponse
    {
        public string Id { get; set; }

        public UserResponse User { get; set; }

        public SchoolResponse School { get; set; }
    }
}
