using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Requests.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IUriService
    {
        public Uri GetModelUri(Dictionary<string, string> identifier, string path);

        public Uri GetPaginationUri(string path, PaginationQuery paginationQuery = null, bool searchMode = false);
    }
}
