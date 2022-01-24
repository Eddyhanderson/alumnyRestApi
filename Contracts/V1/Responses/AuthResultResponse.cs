using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class AuthResultResponse
    {
        public AuthResultResponse() { }

        public UserResponse User { get; set; }

        public AuthConfigTokensResponse AuthConfigTokens { get; set; }

        public bool Authenticated { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public string[] Messages { get; set; }
    }
}
