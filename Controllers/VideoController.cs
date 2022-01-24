using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using alumni.Contracts.V1;
using alumni.Contracts.V1.Requests;
using alumni.Contracts.V1.Requests.Queries;
using alumni.Contracts.V1.Responses;
using alumni.Domain;
using alumni.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace alumni.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IMapper mapper;

        private readonly IVideoService videoService;
            
        private readonly IUriService uriService;

        public VideoController(IMapper mapper, IVideoService videoService, IUriService uriService)
        {
            this.mapper = mapper;

            this.videoService = videoService;

            this.uriService = uriService;
        }

        [HttpPost(ApiRoutes.VideoRoutes.Create)]
        public async Task<IActionResult> Create([FromBody] VideoRequest videoRequest)
        {
            await Task.Run(() => Task.Delay(100));

            return Ok();
        }

        [RequestSizeLimit(737280000)]
        [HttpPost(ApiRoutes.VideoRoutes.Upload)]
        public async Task<IActionResult> Upload(IFormFile file)
        {

            var video = await videoService.CreateTemporaryVideoAsync(file);

            if (video == null) return BadRequest();

            var videoResponse = mapper.Map<VideoResponse>(video);

            return Ok(new Response<VideoResponse>(videoResponse));
        }

        [HttpPost(ApiRoutes.VideoRoutes.VideoWatch)]
        public IActionResult VideoWatch([FromBody] VideoRequest videoRequest, [FromRoute] string connectionId)
        {
            if (videoRequest == null || connectionId == null) return BadRequest();

            var video = mapper.Map<Video>(videoRequest);

            videoService.GetVideoDurationAsync(video.Id, connectionId);
            videoService.GetFrameCountAsync(video.Id, connectionId);
            videoService.GetThumbnailVideoAsync(video.Id, connectionId);
            videoService.GetShortVideoAsync(video.Id);
            videoService.ExecuteDashConversionAsync(video.Id, connectionId);
            return Ok();
        }
      
        [HttpGet(ApiRoutes.VideoRoutes.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
        {
            return NoContent();
        }
    }
}
