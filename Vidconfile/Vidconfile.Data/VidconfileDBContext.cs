using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vidconfile.Data.Contracts;
using Vidconfile.Data.Models;

namespace Vidconfile.Data
{
    public class VidconfileDBContext : DbContext, IVidconfileDBContext
    {
        public VidconfileDBContext(DbContextOptions<VidconfileDBContext> options) 
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VidconfileUserVideo>()
                .HasKey(vu => new { vu.VidconfileUserId, vu.VideoId });

            modelBuilder.Entity<VidconfileUserVideo>()
                .HasOne(bc => bc.Video)
                .WithMany(b => b.LikedUsers)
                .HasForeignKey(bc => bc.VideoId);

            modelBuilder.Entity<VidconfileUserVideo>()
                .HasOne(bc => bc.VidconfileUser)
                .WithMany(c => c.LikedVideos)
                .HasForeignKey(bc => bc.VidconfileUserId);

            modelBuilder.Entity<Video>()
                .HasOne(a => a.Uploader)
                .WithMany(a => a.UploadedVideos)
                .HasForeignKey(a => a.UploaderId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<VidconfileUser> Users { get; set; }
        public DbSet<Video> Videos { get; set; }
    }
}