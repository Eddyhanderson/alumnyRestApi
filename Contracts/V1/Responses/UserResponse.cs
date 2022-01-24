using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }        

        public string PictureProfilePath { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Genre { get; set; }                         

        public string Role { get; set; }

        public string AboutUser { get; set; }
    }
}
