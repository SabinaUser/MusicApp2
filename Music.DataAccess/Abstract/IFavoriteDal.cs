using Music.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Music.Core.DataAccess.IEntityRepository;

namespace Music.DataAccess.Abstract
{
    public interface IFavoriteDal : IEntityRepository<Favorite>
    {
        Task<bool> ExistsAsync(string userId, int musicId);
        Task<Favorite> GetByUserAndMusicAsync(string userId, int musicId);
        Task AddAsync(Favorite favorite);
        Task DeleteAsync(Favorite favorite);
        Task<List<Favorite>> GetUserFavoritesAsync(string userId);
    }
}
