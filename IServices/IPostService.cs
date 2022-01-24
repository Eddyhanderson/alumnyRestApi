using alumni.Domain;
using Alumni.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IPostService
    {
        Task<CreationResult<Post>> CreateAsync(PostsTypes postTypes);

        Task<Post> GetPostAsync(string id);

        Task<IEnumerable<Post>> GetPostsAsync(PaginationFilter filter = null);

        Task<bool> ObjectExists(string id);
    }
}
