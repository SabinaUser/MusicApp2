using Music.Shared.Dtos;
using Music.Shared.Entities;

namespace MusicService.Services.Abstract
{
    public interface IMusicService
    {
        Task<List<MusicDto>> GetAllAsync();
        Task<MusicDto> GetByIdAsync(int id);
        Task<List<Musicc>> GetUserMusicsAsync(string userId);
        Task AddAsync(IFormFile file, IFormFile? poster, string title, string artist, string userId);
        Task DeleteAsync(int id, string userId);

        Task AddFavoriteAsync(string userId, int musicId);
        Task RemoveFavoriteAsync(string userId, int musicId);
        Task<bool> IsFavoritedAsync(string userId, int musicId);
        Task<List<FavoriteDto>> GetUserFavoritesAsync(string userId);

        Task<(byte[] FileContent, string ContentType, string FileName)> DownloadMusicAsync(int musicId);
       // Task AddCommentAsync(Comment comment);



    }
}
