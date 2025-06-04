using Microsoft.EntityFrameworkCore;
using Music.Core.DataAccess.EntityFramework;
using Music.DataAccess.Abstract;
using Music.Shared.Data;
using Music.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.DataAccess.Concrete.EfEntityFramework
{
    public class EfFavoriteDal : EfEntityRepositoryBase<Favorite, MusicDbContext>, IFavoriteDal
    {
        private readonly MusicDbContext _context;
        public EfFavoriteDal(MusicDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string userId, int musicId)
        {
            return await _context.Favorites.AnyAsync(f => f.UserId == userId && f.MusicId == musicId);
        }

        public async Task<Favorite> GetByUserAndMusicAsync(string userId, int musicId)
        {
            return await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.MusicId == musicId);
        }

        public async Task AddAsync(Favorite favorite)
        {
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Favorite favorite)
        {
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Favorite>> GetUserFavoritesAsync(string userId)
        {
            return await GetList(
                filter: f => f.UserId == userId,
                include: query => query.Include(f => f.Music)
            );
        }

       
    }
}
