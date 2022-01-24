
using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Requests.Queries;
using alumni.Contracts.V1.Responses;
using alumni.Domain;
using alumni.IServices;
using System.Collections.Generic;
using System.Linq;

namespace Alumni.Helpers.PaginationHelpers
{
    public class PaginationHelpers
    {
        public static PageResponse<T> CreatePaginationResponse<T>(
            PaginationFilter paginationFilter, 
            IEnumerable<T> response, 
            IUriService uriService, 
            string path, 
            bool searchMode = false)
        {            

            var nextPage = paginationFilter.PageNumber >= 1 ? uriService.GetPaginationUri(
                path,
                new PaginationQuery
                {
                    PageNumber = paginationFilter.PageNumber + 1,
                    PageSize = paginationFilter.PageSize,
                    SearchValue = paginationFilter.SearchValue
                }, searchMode).ToString() : string.Empty;

            var previousPage = paginationFilter.PageNumber > 1 ? uriService.GetPaginationUri(
                path,
                new PaginationQuery
                {
                    PageNumber = paginationFilter.PageNumber - 1,
                    PageSize = paginationFilter.PageSize,
                    SearchValue = paginationFilter.SearchValue
                }, searchMode).ToString() : string.Empty;

            return new PageResponse<T>
            {
                Data = response,
                NextPage = nextPage.Any() ? nextPage : string.Empty,
                PreviousPage = previousPage.Any() ? previousPage : string.Empty,
                PageNumber = paginationFilter.PageNumber >= 1 ? paginationFilter.PageNumber : (int?)null,
                PageSize = paginationFilter.PageSize >= 1 ? paginationFilter.PageSize : (int?)null
            };
        }          
    }
}
