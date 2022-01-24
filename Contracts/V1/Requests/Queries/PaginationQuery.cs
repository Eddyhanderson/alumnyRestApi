using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests.Queries
{
    public class PaginationQuery
    {
        public PaginationQuery()
        {
            PageNumber = 1;

            PageSize = 50;

            SearchValue = null;
        }

        public PaginationQuery(int pageNumber, int pageSize, string value)
        {
            PageNumber = pageNumber < 1 ? pageNumber : 1;

            PageSize = pageSize > 10 ? 10 : pageSize;

            SearchValue = value;
        }

        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }

        public string SearchValue { get; set; }
    }
}
