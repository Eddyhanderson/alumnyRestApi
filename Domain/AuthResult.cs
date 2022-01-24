using alumni.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class AuthResult
    {
        public AuthResult() { }
        public User User { get; set; }

        public AuthConfigTokens AuthConfigTokens { get; set; }
        
        public bool Authenticated { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public string[] Messages { get; set; }

    }
}
