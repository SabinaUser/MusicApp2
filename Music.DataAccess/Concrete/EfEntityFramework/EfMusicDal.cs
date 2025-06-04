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
    public class EfMusicDal : EfEntityRepositoryBase<Musicc, MusicDbContext>, IMusicDal
    {
        private readonly MusicDbContext _context;
        public EfMusicDal(MusicDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Musicc>> GetAllAsync() =>
        await _context.Musics
            .Include(m => m.UploadedBy)
            .Include(m => m.Comments)
            .Include(m => m.Favorites)
            .Include(m => m.PlaylistMusics)
            .Include(m => m.Downloads)
            .ToListAsync();

        public async Task<Musicc> GetByIdAsync(int id) =>
            await _context.Musics
                .Include(m => m.UploadedBy)
                .Include(m => m.Comments)
                .Include(m => m.Favorites)
                .Include(m => m.PlaylistMusics)
                .Include(m => m.Downloads)
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<List<Musicc>> GetByUserIdAsync(string userId) =>
            await _context.Musics.Where(m => m.UploadedById == userId).ToListAsync();

        public async Task AddAsync(Musicc music)
        {
            _context.Musics.Add(music);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Musicc music)
        {
            _context.Musics.Remove(music);
            await _context.SaveChangesAsync();
        }
    }
}
