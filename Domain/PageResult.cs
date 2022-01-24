using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{
    public class PageResult<T>
    {
        public PageResult() { }

        public IEnumerable<T> Data { get; set; }

        public int? TotalElements { get; set; }

        public PageResult(IEnumerable<T> data)
        {
            Data = data;
        }
    }
}
