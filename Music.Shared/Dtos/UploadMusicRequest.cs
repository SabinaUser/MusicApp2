using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Shared.Dtos
{
    public class UploadMusicRequest
    {
        public IFormFile File { get; set; }
        public IFormFile? Poster { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
    }
}
