using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface ICommentableService
    {
        Task<CreationResult<Commentable>> CreateAsync();
    }
}
