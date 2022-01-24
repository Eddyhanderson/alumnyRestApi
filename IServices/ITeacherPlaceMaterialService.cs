using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface ITeacherPlaceMaterialService
    {
        Task<CreationResult<TeacherPlaceMaterial>> CreateAsync(TeacherPlaceMaterial material);

        Task<TeacherPlaceMaterial> GetTeacherPlaceMaterialAsync(string id);

        Task<TeacherPlaceMaterial> GetTeacherPlaceMaterialByPostAsync(string postId);

        Task<IEnumerable<TeacherPlaceMaterial>> GetMaterialsByTeacherPlaceAsync(string teacherPlaceId, PaginationFilter filter = null);

        Task<bool> ObjectExists(string id);
    }
}
