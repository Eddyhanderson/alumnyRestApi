using alumni.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IVideoService
    {

        Task<Video> CreateTemporaryVideoAsync(IFormFile file);       
        public void GetVideoDurationAsync(string videoId, string connectionId);
        public void GetFrameCountAsync(string videoId, string connectionId);
        public void GetShortVideoAsync(string videoId);
        public void GetThumbnailVideoAsync(string videoId, string connectionId);
        public void ExecuteDashConversionAsync(string videoId, string connectionId);
    }
}
