using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Requests.Queries;
using alumni.IServices;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string Base)
        {
            _baseUri = Base;
        }

        public Uri GetModelUri(Dictionary<string, string> identifier, string path)
        {
            foreach (KeyValuePair<string, string> i in identifier)
            {
                var key = i.Key;

                var id = i.Value;

                path = path.Replace(key, id);
            }

            return new Uri(string.Concat(_baseUri, path));
        }

        public Uri GetPaginationUri(string path, PaginationQuery paginationQuery = null, bool searchMode = false)
        {
            if (paginationQuery == null) return new Uri(String.Concat(_baseUri, path));

            var completeBaseUri = string.Concat(_baseUri, path);

            var modifiedUri = QueryHelpers.AddQueryString(completeBaseUri, "PageNumber", paginationQuery.PageNumber.ToString());

            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "PageSize", paginationQuery.PageSize.ToString());

            if (searchMode)
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "SearchValue", paginationQuery.SearchValue.ToString());

            return new Uri(modifiedUri);
        }

    }
}
