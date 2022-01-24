using alumni.Data;
using alumni.Domain;
using alumni.IServices;
using Alumni.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Services
{
    public class TeacherPlaceMaterialService : ITeacherPlaceMaterialService
    {
        private readonly DataContext dataContext;

        private readonly IPostService postService;

        private readonly IServiceProvider serviceProvider;

        public TeacherPlaceMaterialService(DataContext dataContext,
            IPostService postService,
            IServiceProvider serviceProvider)
        {
            this.dataContext = dataContext;

            this.postService = postService;

            this.serviceProvider = serviceProvider;
        }

        public async Task<CreationResult<TeacherPlaceMaterial>> CreateAsync(TeacherPlaceMaterial material)
        {
            if (material == null) return null;

            var postStt = await postService.CreateAsync(PostsTypes.TeacherMaterial);

            if (!postStt.Succeded) FailCreation();

            material.PostId = postStt.Data.Id;

            var teacher = await dataContext.Teachers.SingleOrDefaultAsync(t => t.UserId == GetUserIdRequest());

            if (teacher == null) return null;

            if (material.TeacherPlaceId == null)
                return null;

            var isOwner = (await dataContext
                    .TeacherPlaces.SingleOrDefaultAsync(tp => tp.Id == material.TeacherPlaceId && tp.TeacherId == teacher.Id)) != null;

            if (!isOwner) return null;

            try
            {
                var newMaterial = new TeacherPlaceMaterial
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = material.Description,
                    MaterialPath = material.MaterialPath,
                    TeacherPlaceId = material.TeacherPlaceId,
                    Title = material.Title,
                    PostId = material.PostId
                };

                await dataContext.TeacherPlaceMaterials.AddAsync(newMaterial);

                await dataContext.SaveChangesAsync();

                return new CreationResult<TeacherPlaceMaterial>
                {
                    Data = newMaterial,
                    Succeded = true
                };
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);

                return FailCreation();
            }
        }

        public async Task<TeacherPlaceMaterial> GetTeacherPlaceMaterialAsync(string id)
        {
            if (id == null) return null;

            var material = await dataContext.TeacherPlaceMaterials
                .Include(tpm => tpm.Post)
                .SingleOrDefaultAsync(s => s.Id == id && s.Post.Situation == Constants.SituationsObjects.NormalSituation);

            return material;
        }

        public async Task<TeacherPlaceMaterial> GetTeacherPlaceMaterialByPostAsync(string postId)
        {
            if (postId == null) return null;

            var material = await (from tpm in dataContext.TeacherPlaceMaterials
                                  from p in dataContext.Posts
                                  where tpm.PostId == postId && p.Id == postId && p.Situation == Constants.SituationsObjects.NormalSituation
                                  select tpm).SingleOrDefaultAsync();

            return material;
        }

        public async Task<IEnumerable<TeacherPlaceMaterial>> GetMaterialsByTeacherPlaceAsync(string teacherPlaceId, PaginationFilter filter = null)
        {
            var materials = dataContext
                .TeacherPlaceMaterials
                .Include(tpm => tpm.Post)
                .Where(tpm => tpm.TeacherPlaceId == teacherPlaceId && tpm.Post.Situation == Constants.SituationsObjects.NormalSituation);

            return await GetPaginationAsync(materials, filter);
        }

        public async Task<bool> ObjectExists(string id)
        {
            return await dataContext.TeacherPlaceMaterials.AnyAsync(tpm => tpm.Id == id);
        }

        private async Task<IEnumerable<TeacherPlaceMaterial>> GetPaginationAsync(IQueryable<TeacherPlaceMaterial> materials, PaginationFilter filter)
        {
            if (filter == null) return await materials.ToListAsync();

            var searchMode = filter.SearchValue != null;

            if (searchMode)
            {
                var sv = filter.SearchValue;

                materials = materials
                    .Where(m => m.Description.Contains(sv) || m.Title.Contains(sv));
            }

            if (filter.PageNumber >= 0 || filter.PageSize > 0)
            {
                var skip = (filter.PageNumber - 1) * filter.PageSize;

                materials = materials.Skip(skip).Take(filter.PageSize);
            }

            return await materials.ToListAsync();
        }

        private CreationResult<TeacherPlaceMaterial> FailCreation()
        {
            return new CreationResult<TeacherPlaceMaterial>
            {
                Succeded = false,
                Errors = new[] { Constants.ModelMessages.FailModelCreated }
            };
        }

        private string GetUserIdRequest()
        {
            var accessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

            var user = accessor.HttpContext.GetUser();

            return user;
        }
    }
}
