using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Contracts.V1.Requests
{
    public class VideoRequest
    {
        public string Id { get; set; }

        public string FileName { get; set; }
    }
}
