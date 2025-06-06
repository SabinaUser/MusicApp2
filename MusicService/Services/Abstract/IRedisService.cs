namespace MusicService.Services.Abstract
{
    public interface IRedisService
    {
        Task AddToFavouritesAsync(string userId, int musicId);
        Task RemoveFromFavouritesAsync(string userId,int musicId);

        Task<List<int>> GetAllFavouritesAsync(string userId);

    }
}
