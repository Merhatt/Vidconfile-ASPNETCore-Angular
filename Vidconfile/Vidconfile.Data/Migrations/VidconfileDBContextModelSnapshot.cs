﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Vidconfile.Data;

namespace Vidconfile.Data.Migrations
{
    [DbContext(typeof(VidconfileDBContext))]
    partial class VidconfileDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932");

            modelBuilder.Entity("Vidconfile.Data.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("AuthorId");

                    b.Property<string>("CommentText");

                    b.Property<DateTime>("Created");

                    b.Property<Guid?>("VideoId");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("VideoId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Vidconfile.Data.Models.SubscribeToSubscribers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("SubscribedToId");

                    b.Property<Guid>("SubscriberId");

                    b.HasKey("Id");

                    b.ToTable("SubscribeToSubscribers");
                });

            modelBuilder.Entity("Vidconfile.Data.Models.VidconfileUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("ProfilePhotoUrl");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Vidconfile.Data.Models.Video", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<string>("ThumbnailUrl");

                    b.Property<string>("Title");

                    b.Property<Guid?>("UploaderId");

                    b.Property<byte[]>("VideoData");

                    b.Property<ulong>("Views");

                    b.HasKey("Id");

                    b.HasIndex("UploaderId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("Vidconfile.Data.Models.Comment", b =>
                {
                    b.HasOne("Vidconfile.Data.Models.VidconfileUser", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId");

                    b.HasOne("Vidconfile.Data.Models.Video", "Video")
                        .WithMany("Comments")
                        .HasForeignKey("VideoId");
                });

            modelBuilder.Entity("Vidconfile.Data.Models.Video", b =>
                {
                    b.HasOne("Vidconfile.Data.Models.VidconfileUser", "Uploader")
                        .WithMany("UploadedVideos")
                        .HasForeignKey("UploaderId");
                });
#pragma warning restore 612, 618
        }
    }
}