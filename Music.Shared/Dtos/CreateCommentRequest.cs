using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Shared.Dtos
{
    public class CreateCommentRequest
    {
        public int MusicId { get; set; }
        public string Text { get; set; } = null!;
    }
}
