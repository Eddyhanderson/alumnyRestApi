using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class PageResponse<T>
    {
        public PageResponse() { }
        public IEnumerable<T> Data { get; set; }

        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }

        public int? TotalElements { get; set; }

        public string NextPage { get; set; }

        public string PreviousPage { get; set; }

        public PageResponse(IEnumerable<T> data)
        {
            Data = data;
        }
    }
}
