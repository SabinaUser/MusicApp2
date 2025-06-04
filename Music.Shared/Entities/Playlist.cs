using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Shared.Entities
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<PlaylistMusic> PlaylistMusics { get; set; }
    }
}
