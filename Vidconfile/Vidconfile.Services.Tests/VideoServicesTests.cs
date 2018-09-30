using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Vidconfile.Data.Contracts;
using Vidconfile.Data.Models;
using Vidconfile.Services.Services;
using Xunit;

namespace Vidconfile.Services.Tests
{
    public class VideoServicesTests
    {
        [Fact]
        public void ChangeViewsOfVideo_NullVideo_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();

            VideoServices videoService = new VideoServices(videoRepositoryMock.Object, unitOfWorkMock.Object, commentRepositoryMock.Object);

            Video video = null;
            ulong views = 0;

            string message = Assert
                .Throws<NullReferenceException>(() => videoService.ChangeViewsOfVideo(video, views))
                .Message;

            Assert.Equal("video cannot be null", message);
        }

        [Fact]
        public void ChangeViewsOfVideo_ShouldChangeViews()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();

            VideoServices videoService = new VideoServices(videoRepositoryMock.Object, unitOfWorkMock.Object, commentRepositoryMock.Object);

            Video video = new Video();
            ulong views = 2;

            videoService.ChangeViewsOfVideo(video, views);

            videoRepositoryMock.Verify(x => x.Update(video), Times.Once);
            unitOfWorkMock.Verify(x => x.Commit(), Times.Once);

            Assert.Equal(views, video.Views);
        }

        [Fact]
        public void GetAllVideos_ShouldGetAll()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();

            IQueryable<Video> videos = new List<Video>().AsQueryable();

            videoRepositoryMock.Setup(x => x.All())
                .Returns(videos);

            VideoServices videoService = new VideoServices(videoRepositoryMock.Object, unitOfWorkMock.Object, commentRepositoryMock.Object);

            var allVideos = videoService.GetAllVideos();

            videoRepositoryMock.Verify(x => x.All(), Times.Once());

            Assert.Same(videos, allVideos);
        }

        [Fact]
        public void GetVideoById_ShouldGetById()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();

            Video video = new Video();

            Guid id = Guid.NewGuid();

            videoRepositoryMock.Setup(x => x.GetById(id))
                .Returns(video);

            VideoServices videoService = new VideoServices(videoRepositoryMock.Object, unitOfWorkMock.Object, commentRepositoryMock.Object);

            var newVideo = videoService.GetVideoById(id);

            videoRepositoryMock.Verify(x => x.GetById(id), Times.Once());
            Assert.Same(video, newVideo);
        }

        [Fact]
        public void GetVideoByIdWithComments_ShouldGetById()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();

            var videos = new List<Video>();

            Guid id = Guid.NewGuid();

            var video = new Video();
            video.Id = id;

            videos.Add(video);

            videoRepositoryMock.Setup(x => x.All("Comments.Author"))
                .Returns(videos.AsQueryable());

            VideoServices videoService = new VideoServices(videoRepositoryMock.Object, unitOfWorkMock.Object, commentRepositoryMock.Object);

            var newVideo = videoService.GetVideoByIdWithComments(id);

            videoRepositoryMock.Verify(x => x.All("Comments.Author"), Times.Once());
            Assert.Same(video, newVideo);
        }

        [Fact]
        public void UploadVideo_NullDescription_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();

            var videos = new List<Video>();

            Guid id = Guid.NewGuid();

            VideoServices videoService = new VideoServices(videoRepositoryMock.Object, unitOfWorkMock.Object, commentRepositoryMock.Object);

            VidconfileUser user = new VidconfileUser();
            byte[] videoData = new byte[4];
            string thumbnailUrl = "asd";
            string title = "test";

            string message = Assert.Throws<NullReferenceException>(() => videoService.UploadVideo(user, videoData, null, thumbnailUrl, title))
                .Message;
            
            Assert.Same("description cannot be null", message);
        }

        [Fact]
        public void UploadVideo_NullThumbnailUrl_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();

            var videos = new List<Video>();

            Guid id = Guid.NewGuid();

            VideoServices videoService = new VideoServices(videoRepositoryMock.Object, unitOfWorkMock.Object, commentRepositoryMock.Object);

            VidconfileUser user = new VidconfileUser();
            byte[] videoData = new byte[4];
            string thumbnailUrl = null;
            string description = "hi";
            string title = "test";

            string message = Assert.Throws<NullReferenceException>(() => videoService.UploadVideo(user, videoData, description, thumbnailUrl, title))
                .Message;

            Assert.Same("thumbnailUrl cannot be null", message);
        }

        [Fact]
        public void UploadVideo_TitleUrlNull_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();

            var videos = new List<Video>();

            Guid id = Guid.NewGuid();

            VideoServices videoService = new VideoServices(videoRepositoryMock.Object, unitOfWorkMock.Object, commentRepositoryMock.Object);

            VidconfileUser user = new VidconfileUser();
            byte[] videoData = new byte[4];
            string thumbnailUrl = "far";
            string description = "hi";
            string title = null;

            string message = Assert.Throws<NullReferenceException>(() => videoService.UploadVideo(user, videoData, description, thumbnailUrl, title))
                .Message;

            Assert.Same("title cannot be null", message);
        }

        [Fact]
        public void UploadVideo_UploaderNull_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();

            var videos = new List<Video>();

            Guid id = Guid.NewGuid();

            VideoServices videoService = new VideoServices(videoRepositoryMock.Object, unitOfWorkMock.Object, commentRepositoryMock.Object);

            VidconfileUser user = null;
            byte[] videoData = new byte[4];
            string thumbnailUrl = "far";
            string description = "hi";
            string title = "fasd";

            string message = Assert.Throws<NullReferenceException>(() => videoService.UploadVideo(user, videoData, description, thumbnailUrl, title))
                .Message;

            Assert.Same("uploader cannot be null", message);
        }

        [Fact]
        public void UploadVideo_VideoDataNull_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();

            var videos = new List<Video>();

            Guid id = Guid.NewGuid();

            VideoServices videoService = new VideoServices(videoRepositoryMock.Object, unitOfWorkMock.Object, commentRepositoryMock.Object);

            VidconfileUser user = new VidconfileUser();
            byte[] videoData = null;
            string thumbnailUrl = "far";
            string description = "hi";
            string title = "fasd";

            string message = Assert.Throws<NullReferenceException>(() => videoService.UploadVideo(user, videoData, description, thumbnailUrl, title))
                .Message;

            Assert.Same("videoData cannot be null", message);
        }

        [Fact]
        public void UploadVideo_ShouldCreateVideo()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();

            Video video = null;

            videoRepositoryMock.Setup(x => x.Add(It.IsAny<Video>()))
                .Callback<Video>(r => video = r);;

            Guid id = Guid.NewGuid();

            VideoServices videoService = new VideoServices(videoRepositoryMock.Object, unitOfWorkMock.Object, commentRepositoryMock.Object);

            VidconfileUser user = new VidconfileUser();
            byte[] videoData = new byte[2];
            string thumbnailUrl = "far";
            string description = "hi";
            string title = "fasd";

            videoService.UploadVideo(user, videoData, description, thumbnailUrl, title);

            Assert.Same(user, video.Uploader);
            Assert.Same(videoData, video.VideoData);
            Assert.Same(thumbnailUrl, video.ThumbnailUrl);
            Assert.Same(description, video.Description);
            Assert.Same(title, video.Title);

            videoRepositoryMock.Verify(x => x.Add(It.IsAny<Video>()), Times.Once());
            unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
        }
    }
}
