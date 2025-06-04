using FavoriteService.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Music.DataAccess.Abstract;
using System.Security.Claims;

namespace FavoriteService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public FavoriteController(IFavoriteService favoriteService, IHttpContextAccessor httpContextAccessor)
        {
            _favoriteService = favoriteService;
            _httpContextAccessor = httpContextAccessor;
        }
        private string GetUserId() =>
       _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      

        [HttpPost("{musicId}")]
        public async Task<IActionResult> AddToFavorite(int musicId)
        {
            var userId = GetUserId();
            await _favoriteService.AddFavoriteAsync(userId, musicId);
            return Ok(new { message = "Musiqi favoritlərə əlavə olundu." });
        }

        [HttpDelete("{musicId}")]
        public async Task<IActionResult> RemoveFromFavorite(int musicId)
        {
            var userId = GetUserId();
            await _favoriteService.RemoveFavoriteAsync(userId, musicId);
            return Ok(new { message = "Musiqi favoritlərdən silindi." });
        }

        [HttpGet("is-favorited/{musicId}")]
        public async Task<IActionResult> IsFavorited(int musicId)
        {
            var userId = GetUserId();
            var isFav = await _favoriteService.IsFavoritedAsync(userId, musicId);
            return Ok(isFav);
        }

        [HttpGet("my")]
        public async Task<IActionResult> MyFavorites()
        {
            var userId = GetUserId();
            var list = await _favoriteService.GetUserFavoritesAsync(userId);
            return Ok(list);
        }

    }
}
