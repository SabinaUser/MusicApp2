using FavoriteService.Services.Abstract;
using Music.DataAccess.Abstract;
using Music.Shared.Dtos;
using Music.Shared.Entities;

namespace FavoriteService.Services.Concrete
{
    public class FavoriteService:IFavoriteService
    {
        private readonly IFavoriteDal _favoriteDal;
        private readonly IMusicDal _musicDal;

        public FavoriteService(IFavoriteDal favoriteDal, IMusicDal musicDal)
        {
            _favoriteDal = favoriteDal;
            _musicDal = musicDal;
        }

        public async Task AddFavoriteAsync(string userId, int musicId)
        {
            var exists = await _favoriteDal.ExistsAsync(userId, musicId);
            if (exists)
                throw new InvalidOperationException("Bu musiqi artıq favoritlər siyahısına əlavə olunub.");

            var favorite = new Favorite
            {
                UserId = userId,
                MusicId = musicId,
                FavoritedAt = DateTime.UtcNow
            };

            await _favoriteDal.AddAsync(favorite);
        }

        public async Task RemoveFavoriteAsync(string userId, int musicId)
        {
            var favorite = await _favoriteDal.GetByUserAndMusicAsync(userId, musicId);
            if (favorite == null)
                throw new InvalidOperationException("Bu musiqi favoritlərdə yoxdur.");

            await _favoriteDal.DeleteAsync(favorite);
        }

        public async Task<bool> IsFavoritedAsync(string userId, int musicId) =>
            await _favoriteDal.ExistsAsync(userId, musicId);

        public async Task<List<FavoriteDto>> GetUserFavoritesAsync(string userId)
        {
            var favorites = await _favoriteDal.GetUserFavoritesAsync(userId);

            return favorites.Select(f => new FavoriteDto
            {
                MusicId = f.MusicId,
                Title = f.Music.Title,
                Artist = f.Music.Artist,
                PosterImagePath = f.Music.PosterImagePath,
                FilePath = f.Music.FilePath,
                FavoritedAt = f.FavoritedAt
            }).ToList();
        }


    }
}
