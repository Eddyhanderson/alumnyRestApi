using alumni.Contracts.V1.Requests.Queries;
using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface ICommentService
    {
        Task<CreationResult<Comment>> CreateAsync(Comment article);

        Task<PageResult<Comment>> GetCommentsAsync(CommentQuery query, PaginationFilter filter = null);

        Task<Comment> GetCommentAsync(string id);
    }
}
