using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Shared.Entities
{
    public class PlaylistMusic
    {
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public int MusicId { get; set; }
        public Musicc Music { get; set; }

        public DateTime AddedAt { get; set; }
    }
}
