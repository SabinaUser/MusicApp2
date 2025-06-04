using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Music.Shared.Dtos;
using MusicService.Services.Abstract;
using System.Security.Claims;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService _musicService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MusicController(IMusicService musicService, IHttpContextAccessor httpContextAccessor)
        {
            _musicService = musicService;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetUserId() =>
        _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _musicService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) =>
            Ok(await _musicService.GetByIdAsync(id));

        [HttpGet("my")]
        public async Task<IActionResult> GetMyMusics()
        {
            var userId = GetUserId();
            return Ok(await _musicService.GetUserMusicsAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] UploadMusicRequest request)
        {
            var userId = GetUserId();
            await _musicService.AddAsync(request.File, request.Poster, request.Title, request.Artist, userId);
            return Ok(new { message = "Musiqi uğurla yükləndi." });
        }

        [HttpDelete("delete-music/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            await _musicService.DeleteAsync(id, userId);
            return Ok(new { message = "Musiqi silindi." });
        }

        [HttpPost("{musicId}")]
        public async Task<IActionResult> AddToFavorite(int musicId)
        {
            var userId = GetUserId();
            await _musicService.AddFavoriteAsync(userId, musicId);
            return Ok(new { message = "Musiqi favoritlərə əlavə olundu." });
        }

        [HttpDelete("{musicId}")]
        public async Task<IActionResult> RemoveFromFavorite(int musicId)
        {
            var userId = GetUserId();
            await _musicService.RemoveFavoriteAsync(userId, musicId);
            return Ok(new { message = "Musiqi favoritlərdən silindi." });
        }

        [HttpGet("is-favorited/{musicId}")]
        public async Task<IActionResult> IsFavorited(int musicId)
        {
            var userId = GetUserId();
            var isFav = await _musicService.IsFavoritedAsync(userId, musicId);
            return Ok(isFav);
        }

        [HttpGet("my-favorites")]
        public async Task<IActionResult> MyFavorites()
        {
            var userId = GetUserId();
            var list = await _musicService.GetUserFavoritesAsync(userId);
            return Ok(list);
        }
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var (fileContent, contentType, fileName) = await _musicService.DownloadMusicAsync(id);
            return File(fileContent, contentType, fileName);
        }


    }
}
