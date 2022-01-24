using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface ITeacherPlaceMessageService
    {
        Task<CreationResult<TeacherPlaceMessage>> CreateAsync(TeacherPlaceMessage message);

        Task<TeacherPlaceMessage> GetTeacherPlaceMessageAsync(string id);

        Task<IEnumerable<TeacherPlaceMessage>> GetMessagesByTeacherPlaceAsync(string teacherPlaceId, PaginationFilter filter = null);

        Task<bool> ObjectExists(string id);

        Task<TeacherPlaceMessage> GetTeacherPlaceMessageByPostAsync(string postId);
    }
}
