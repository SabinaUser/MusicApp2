using Music.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Shared.Entities
{
    public class Musicc:IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string FilePath { get; set; }     
        public string? PosterImagePath { get; set; } 
        public DateTime UploadedAt { get; set; }

        public string UploadedById { get; set; }
        public ApplicationUser UploadedBy { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<PlaylistMusic> PlaylistMusics { get; set; }
        public ICollection<Download> Downloads { get; set; }
    }
}
