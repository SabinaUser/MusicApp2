using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Shared.Dtos
{
    public class FavoriteDto
    {
        public int MusicId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string PosterImagePath { get; set; }
        public string FilePath { get; set; }
        public DateTime FavoritedAt { get; set; }
    }
}
