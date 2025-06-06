using MusicService.Services.Abstract;
using StackExchange.Redis;

namespace MusicService.Services.Concrete
{
    public class RedisService:IRedisService
    {
        private readonly IDatabase _database;
        public RedisService(IConnectionMultiplexer _redisMultiplexer)
        {
            _database = _redisMultiplexer.GetDatabase();
        }
        public async Task AddToFavouritesAsync(string userId, int musicId)
        {
            await _database.SetAddAsync($"user:{userId}:favourites", musicId);
        }
        
        public async Task<List<int>> GetAllFavouritesAsync(string userId)
        {
            var items = await _database.SetMembersAsync($"user:{userId}:favourites");

            return items.Select(item => (int)item).ToList();
        }

        public Task RemoveFromFavouritesAsync(string userId, int musicId)
        {
            return _database.SetRemoveAsync($"user:{userId}:favourites", musicId);
        }
    }
}
