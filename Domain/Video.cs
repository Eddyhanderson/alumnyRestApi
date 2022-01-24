using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Domain
{ 
    public class Video
    {
        [Key]
        [Required]
        public string Id { get; set; }
        
        public string Duration { get; set; }

        public int FrameLength { get; set; }    
                
        public string TempFileName { get; set; }

        public string ManifestPath { get; set; }

        [Required]
        public DateTime CrationAt { get; set; }

        public string ThumbnailPath { get; set; }

        public string ShortVideoPath { get; set; }

        [Required]        
        public bool Converted { get; set; }
    }
}
