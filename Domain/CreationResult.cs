using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class CreationResult<T>
    {
        public bool Succeded { get; set; }

        public bool Exists { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public IEnumerable<string> Messages { get; set; }

        public T Data { get; set; }

        public CreationResult(T data)
        {
            Data = data;
        }

        public CreationResult() { }
    }
}
