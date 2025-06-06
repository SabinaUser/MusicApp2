using Music.DataAccess.Abstract;
using Music.DataAccess.Services;
using Music.Shared.Dtos;
using Music.Shared.Entities;
using MusicService.Services.Abstract;

namespace MusicService.Services.Concrete
{
    public class MusiccService:IMusicService
    {
        private readonly IMusicDal _musicDal;
        private readonly IFileService _fileService;
        private readonly IFavoriteDal _favoriteDal;
        private readonly IRedisService _redisService;

        public MusiccService(IMusicDal musicDal, IFileService fileService, IFavoriteDal favoriteDal, IRedisService redisService)
        {
            _musicDal = musicDal;
            _fileService = fileService;
            _favoriteDal = favoriteDal;
            _redisService = redisService;
        }

        //public async Task<List<Musicc>> GetAllAsync() =>
        //    await _musicDal.GetAllAsync();

        public async Task<List<MusicDto>> GetAllAsync()
        {
            var musics = await _musicDal.GetAllAsync();
            return musics.Select(m => new MusicDto
            {
                Id = m.Id,
                Title = m.Title,
                Artist = m.Artist,
                FilePath = m.FilePath,
                PosterImagePath = m.PosterImagePath,
                UploadedAt = m.UploadedAt
            }).ToList();
        }


        public async Task<MusicDto> GetByIdAsync(int id)
        {
            var music = await _musicDal.GetByIdAsync(id);
            if (music == null)
                throw new Exception("Musiqi tapılmadı.");

            return new MusicDto
            {
                Id = music.Id,
                Title = music.Title,
                Artist = music.Artist,
                FilePath = music.FilePath,
                PosterImagePath = music.PosterImagePath,
                UploadedAt = music.UploadedAt
            };
        }


        public async Task<List<Musicc>> GetUserMusicsAsync(string userId) =>
            await _musicDal.GetByUserIdAsync(userId);

        public async Task AddAsync(IFormFile file, IFormFile? poster, string title, string artist, string userId)
        {
            var filePath = await _fileService.UploadFileAsync(file);
            string? posterPath = poster != null ? await _fileService.UploadFileAsync(poster) : null;

            var music = new Musicc
            {
                Title = title,
                Artist = artist,
                FilePath = filePath,
                PosterImagePath = posterPath,
                UploadedAt = DateTime.UtcNow,
                UploadedById = userId
            };

            await _musicDal.AddAsync(music);
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var music = await _musicDal.GetByIdAsync(id);
            if (music == null || music.UploadedById != userId)
                throw new UnauthorizedAccessException("Siz bu musiqini silə bilməzsiniz.");

            await _musicDal.DeleteAsync(music);
        }

        //public async Task AddFavoriteAsync(string userId, int musicId)
        //{
        //    var exists = await _favoriteDal.ExistsAsync(userId, musicId);
        //    if (exists)
        //        throw new InvalidOperationException("Bu musiqi artıq favoritlər siyahısına əlavə olunub.");

        //    var favorite = new Favorite
        //    {
        //        UserId = userId,
        //        MusicId = musicId,
        //        FavoritedAt = DateTime.UtcNow
        //    };

        //    await _favoriteDal.AddAsync(favorite);
        //}

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

            // Redis-ə də əlavə et
            await _redisService.AddToFavouritesAsync(userId, musicId);
        }

        //public async Task RemoveFavoriteAsync(string userId, int musicId)
        //{
        //    var favorite = await _favoriteDal.GetByUserAndMusicAsync(userId, musicId);
        //    if (favorite == null)
        //        throw new InvalidOperationException("Bu musiqi favoritlərdə yoxdur.");

        //    await _favoriteDal.DeleteAsync(favorite);
        //}
        public async Task RemoveFavoriteAsync(string userId, int musicId)
        {
            var favorite = await _favoriteDal.GetByUserAndMusicAsync(userId, musicId);
            if (favorite == null)
                throw new InvalidOperationException("Bu musiqi favoritlərdə yoxdur.");

            await _favoriteDal.DeleteAsync(favorite);

            // Redis-dən də sil
            await _redisService.RemoveFromFavouritesAsync(userId, musicId);
        }


        public async Task<bool> IsFavoritedAsync(string userId, int musicId) =>
            await _favoriteDal.ExistsAsync(userId, musicId);

        //public async Task<List<FavoriteDto>> GetUserFavoritesAsync(string userId)
        //{
        //    var favorites = await _favoriteDal.GetUserFavoritesAsync(userId);

        //    return favorites.Select(f => new FavoriteDto
        //    {
        //        MusicId = f.MusicId,
        //        Title = f.Music.Title,
        //        Artist = f.Music.Artist,
        //        PosterImagePath = f.Music.PosterImagePath,
        //        FilePath = f.Music.FilePath,
        //        FavoritedAt = f.FavoritedAt
        //    }).ToList();
        //}
        public async Task<List<FavoriteDto>> GetUserFavoritesAsync(string userId)
        {
            var redisIds = await _redisService.GetAllFavouritesAsync(userId);

            if (redisIds != null && redisIds.Count > 0)
            {
                var musicList = await _musicDal.GetByIdsAsync(redisIds); // Bu metodu DAL səviyyəsində əlavə etməlisən

                return musicList.Select(m => new FavoriteDto
                {
                    MusicId = m.Id,
                    Title = m.Title,
                    Artist = m.Artist,
                    PosterImagePath = m.PosterImagePath,
                    FilePath = m.FilePath,
                    FavoritedAt = DateTime.UtcNow // Redis-də tarix yoxdur, dummy dəyər
                }).ToList();
            }

            // Əgər Redis boşdursa, DB-dən çək və Redis-ə yaz
            var favorites = await _favoriteDal.GetUserFavoritesAsync(userId);

            foreach (var fav in favorites)
                await _redisService.AddToFavouritesAsync(userId, fav.MusicId);

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


        public async Task<(byte[] FileContent, string ContentType, string FileName)> DownloadMusicAsync(int id)
        {
            var music = await _musicDal.GetByIdAsync(id);
            if (music == null)
                throw new Exception("Musiqi tapılmadı.");

            // remove leading "/" to prevent Path.Combine from being overridden
            var relativePath = music.FilePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException("Fayl mövcud deyil: " + filePath);

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var contentType = "audio/mpeg"; // MP3 üçün
            var fileName = Path.GetFileName(filePath);

            return (fileBytes, contentType, fileName);
        }




    }
}
