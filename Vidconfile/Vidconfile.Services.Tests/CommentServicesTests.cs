using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Vidconfile.Data.Contracts;
using Vidconfile.Data.Models;
using Xunit;

namespace Vidconfile.Services.Tests
{
    public class CommentServicesTests
    {
        [Fact]
        public void AddComment_NullComment_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();

            CommentServices commentService = 
                new CommentServices(commentRepositoryMock.Object, unitOfWorkMock.Object, userRepositoryMock.Object, videoRepositoryMock.Object);

            Video video = new Video();
            VidconfileUser user = new VidconfileUser();
            string commentText = "asdasd";

            string message = Assert
                .Throws<NullReferenceException>(() => commentService.AddComment(video, user, null))
                .Message;

            Assert.Equal("commentText cannot be null or empty", message);
        }

        [Fact]
        public void AddComment_NullAuthor_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();

            CommentServices commentService =
                new CommentServices(commentRepositoryMock.Object, unitOfWorkMock.Object, userRepositoryMock.Object, videoRepositoryMock.Object);

            Video video = new Video();
            VidconfileUser user = new VidconfileUser();
            string commentText = "asdasd";

            string message = Assert
                .Throws<NullReferenceException>(() => commentService.AddComment(video, null, commentText))
                .Message;

            Assert.Equal("author cannot be null", message);
        }

        [Fact]
        public void AddComment_NullVideo_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();

            CommentServices commentService =
                new CommentServices(commentRepositoryMock.Object, unitOfWorkMock.Object, userRepositoryMock.Object, videoRepositoryMock.Object);

            Video video = new Video();
            VidconfileUser user = new VidconfileUser();
            string commentText = "asdasd";

            string message = Assert
                .Throws<NullReferenceException>(() => commentService.AddComment(null, user, commentText))
                .Message;

            Assert.Equal("video cannot be null", message);
        }

        [Fact]
        public void AddComment_ShouldAdd()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();

            Comment value = null;

            commentRepositoryMock.Setup(x => x.Add(It.IsAny<Comment>()))
                .Callback<Comment>(x => value = x);

            CommentServices commentService =
                new CommentServices(commentRepositoryMock.Object, unitOfWorkMock.Object, userRepositoryMock.Object, videoRepositoryMock.Object);

            Video video = new Video();
            VidconfileUser user = new VidconfileUser();
            string commentText = "asdasd";

            commentService.AddComment(video, user, commentText);

            Assert.Same(video, value.Video);
            Assert.Same(user, value.Author);
            Assert.Same(commentText, value.CommentText);

            commentRepositoryMock.Verify(x => x.Add(value), Times.Once);
            unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
        }
    }
}
