using System;
using System.ComponentModel.DataAnnotations;

namespace alumni.Contracts.V1.Responses
{
    public class ManagerResponse
    {
        public string Id { get; set; }        
        public string FirstName { get; set; }        
        public string LastName { get; set; }
    }
}
