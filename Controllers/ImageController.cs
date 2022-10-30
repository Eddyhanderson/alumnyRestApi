using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using alumni.Contracts.V1;
using alumni.Contracts.V1.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace alumni.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ImageController : Controller
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<ImageController> logger;
        private const string DEFAULT_DIR = "images\\imageProfile\\";

        public ImageController(IWebHostEnvironment env, ILogger<ImageController> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        [HttpPost(ApiRoutes.ImagesRoutes.UploadLessonProfile)]
        public async Task<IActionResult> UploadLessonImage(IFormFile file)
        {
            if (file == null) return BadRequest();

            var root = "lessons";

            var name = Path.GetRandomFileName();

            var ext = Path.GetFileName(file.FileName).Split('.').Last();

            var pathDir = Path.Combine(env.WebRootPath, DEFAULT_DIR, root, name);

            var fileName = $"{name}.{ext}";

            try
            {
                Directory.CreateDirectory(pathDir);

                using (var stream = new FileStream(Path.Combine(pathDir, fileName), FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(stream);
                }

                var result = new Response<string>(Path.Combine(DEFAULT_DIR, root, name, fileName));

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);

                return BadRequest();
            }
        }

        [HttpPost(ApiRoutes.ImagesRoutes.UploadTopicImage)]
        public async Task<IActionResult> UploadTopicImage(IFormFile file)
        {
            if (file == null) return BadRequest();

            var root = "topics";

            var name = Path.GetRandomFileName();

            var ext = Path.GetFileName(file.FileName).Split('.').Last();

            var pathDir = Path.Combine(env.WebRootPath, DEFAULT_DIR, root, name);

            var fileName = $"{name}.{ext}";

            try
            {
                Directory.CreateDirectory(pathDir);

                using (var stream = new FileStream(Path.Combine(pathDir, fileName), FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(stream);
                }

                var result = new Response<string>(Path.Combine(DEFAULT_DIR, root, name, fileName));

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);

                return BadRequest();
            }
        }

        [HttpPost(ApiRoutes.ImagesRoutes.UploadModuleImage)]
        public async Task<IActionResult> UploadModuleImage(IFormFile file)
        {
            if (file == null) return BadRequest();

            var root = "modules";

            var name = Path.GetRandomFileName();

            var ext = Path.GetFileName(file.FileName).Split('.').Last();

            var pathDir = Path.Combine(env.WebRootPath, DEFAULT_DIR, root, name);

            var fileName = $"{name}.{ext}";

            try
            {
                Directory.CreateDirectory(pathDir);

                using (var stream = new FileStream(Path.Combine(pathDir, fileName), FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(stream);
                }

                var result = new Response<string>(Path.Combine(DEFAULT_DIR, root, name, fileName));

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);

                return BadRequest();
            }
        }

        [HttpPost(ApiRoutes.ImagesRoutes.UploadFormationImage)]
        public async Task<IActionResult> UploadFormationImage(IFormFile file)
        {
            if (file == null) return BadRequest();

            var root = "formation";

            var name = Path.GetRandomFileName();

            var ext = Path.GetFileName(file.FileName).Split('.').Last();

            var pathDir = Path.Combine(env.WebRootPath, DEFAULT_DIR, root, name);

            var fileName = $"{name}.{ext}";

            try
            {
                Directory.CreateDirectory(pathDir);

                using (var stream = new FileStream(Path.Combine(pathDir, fileName), FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(stream);
                }

                var result = new Response<string>(Path.Combine(DEFAULT_DIR, root, name, fileName));

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);

                return BadRequest();
            }
        }

    }
}
