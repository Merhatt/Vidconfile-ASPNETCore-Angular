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
            modelBuilder.Entity<Video>()
                .HasOne(a => a.Uploader)
                .WithMany(a => a.UploadedVideos)
                .HasForeignKey(a => a.UploaderId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Video)
                .WithMany(v => v.Comments)
                .HasForeignKey(c => c.VideoId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(v => v.Comments)
                .HasForeignKey(c => c.AuthorId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<VidconfileUser> Users { get; set; }
        public DbSet<Video> Videos { get; set; }
    }
}