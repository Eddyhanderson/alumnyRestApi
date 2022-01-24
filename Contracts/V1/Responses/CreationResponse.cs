using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class CreationResponse<T>
    {
        public bool Succeded { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public IEnumerable<string> Messages { get; set; }

        public Uri GetUri { get; set; }

        public T Data { get; set; }

        public CreationResponse(T data)
        {
            Data = data;
        }

        public CreationResponse() { }
    }
}
