using Music.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Shared.Entities
{
    public class Favorite:IEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int MusicId { get; set; }
        public Musicc Music { get; set; }

        public DateTime FavoritedAt { get; set; }
    }
}
