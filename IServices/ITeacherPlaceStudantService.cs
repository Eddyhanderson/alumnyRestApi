using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface ITeacherPlaceStudantService
    {
        Task<CreationResult<TeacherPlaceStudants>> CreateAsync(TeacherPlaceStudants teacherPlaceStudants);

        Task<bool> UpdateAsync(string teacherPlaceId, string studantId, TeacherPlaceStudants teacherPlaceStudant);

        Task<TeacherPlaceStudants> GetAsync(string teacherPlaceId, string studantId);

        Task<IEnumerable<TeacherPlace>> GetTeacherPlacesByStudantAsync(string studantId, PaginationFilter filter = null);

        Task<IEnumerable<Studant>> GetStudantsByTeacherPlaceAsync(string teacherPlaceId, PaginationFilter filter = null);

        Task<bool> ObjectExists(string teacherPlaceId, string studantId);
    }
}
