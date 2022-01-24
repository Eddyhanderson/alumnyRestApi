using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Responses
{
    public class VideoResponse
    {
        [Required]
        public string Id { get; set; }
        
        public int Duration { get; set; }

        [Required]
        public string ManifestPath { get; set; }

        public string ThumbnailPath { get; set; }

        public string ShortVideoPath { get; set; }
    }
}
