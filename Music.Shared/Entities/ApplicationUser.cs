
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Shared.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Musicc> UploadedMusic { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<Download> Downloads { get; set; }

    }
}
