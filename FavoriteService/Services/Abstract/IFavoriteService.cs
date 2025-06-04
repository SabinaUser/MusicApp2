using Music.Shared.Dtos;
using Music.Shared.Entities;

namespace FavoriteService.Services.Abstract
{
    public interface IFavoriteService
    {
        Task AddFavoriteAsync(string userId, int musicId);
        Task RemoveFavoriteAsync(string userId, int musicId);
        Task<bool> IsFavoritedAsync(string userId, int musicId);
        Task<List<FavoriteDto>> GetUserFavoritesAsync(string userId);

    }
}
