using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Music.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Music.Shared.Data
{
    public class MusicDbContext : IdentityDbContext<ApplicationUser>
    {
        public MusicDbContext(DbContextOptions<MusicDbContext> options)
        : base(options)
        {
        }
        public DbSet<Musicc> Musics { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistMusic> PlaylistMusics { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Download> Downloads { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PlaylistMusic>()
                .HasKey(pm => new { pm.PlaylistId, pm.MusicId });

            builder.Entity<PlaylistMusic>()
                .HasOne(pm => pm.Playlist)
                .WithMany(p => p.PlaylistMusics)
                .HasForeignKey(pm => pm.PlaylistId);

            builder.Entity<PlaylistMusic>()
                .HasOne(pm => pm.Music)
                .WithMany(m => m.PlaylistMusics)
                .HasForeignKey(pm => pm.MusicId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
       .HasOne(c => c.Music)
       .WithMany(m => m.Comments)
       .HasForeignKey(c => c.MusicId)
       .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Download>()
        .HasOne(d => d.Music)
        .WithMany(m => m.Downloads)
        .HasForeignKey(d => d.MusicId)
        .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Download>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Favorite>()
    .HasOne(f => f.Music)
    .WithMany(m => m.Favorites)
    .HasForeignKey(f => f.MusicId)
    .OnDelete(DeleteBehavior.Restrict);
        }

    }
}