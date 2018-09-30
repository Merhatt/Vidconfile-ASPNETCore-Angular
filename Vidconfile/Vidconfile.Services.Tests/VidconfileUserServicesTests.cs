using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vidconfile.Data.Contracts;
using Vidconfile.Data.Models;
using Vidconfile.Utils.Contracts;
using Vidconfile.Utils.Models;
using Xunit;

namespace Vidconfile.Services.Tests
{
    public class VidconfileUserServicesTests
    {
        [Fact]
        public void GetUserByVideoId_NonExistingVideo_ShoudReturnNull()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();



            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            Guid id = Guid.NewGuid();

            var res = userService.GetUserByVideoId(id);

            Assert.Null(res);
        }

        [Fact]
        public void GetUserByVideoId_ShouldGetUser()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            Guid id = Guid.NewGuid();

            var video = new Video();
            video.Id = id;

            VidconfileUser user = new VidconfileUser();

            video.Uploader = user;

            var videos = new List<Video>();
            videos.Add(video);

            videoRepositoryMock.Setup(x => x.All(s => s.Uploader))
                .Returns(videos.AsQueryable());

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);
            
            var res = userService.GetUserByVideoId(id);

            Assert.Same(user, res);
            videoRepositoryMock.Verify(x => x.All(s => s.Uploader), Times.Once);
        }

        [Fact]
        public void Login_NullUsername_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string username = "test";
            string password = "nub";

            string message = Assert.Throws<NullReferenceException>(() => userService.Login(null, password)).Message;

            Assert.Equal("username cannot be null or empty", message);
        }

        [Fact]
        public void Login_NullPassword_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string username = "test";
            string password = "nub";

            string message = Assert.Throws<NullReferenceException>(() => userService.Login(username, null)).Message;

            Assert.Equal("password cannot be null or empty", message);
        }

        [Fact]
        public void Login_NotExistingUser_ShouldReturnNull()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string username = "test";
            string password = "nub";

            var user = userService.Login(username, password);

            passwordHasherMock.Verify(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Never);

            Assert.Null(user);
        }

        [Fact]
        public void Login_NotWrongPassword_ShouldReturnNull()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();


            VidconfileUser user = new VidconfileUser();

            string username = "test";
            string password = "nub";

            user.Username = username;

            userRepositoryMock.Setup(x => x.All())
                .Returns(new List<VidconfileUser>() { user }.AsQueryable());

            passwordHasherMock.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(false);

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            var userRet = userService.Login(username, password);

            passwordHasherMock.Verify(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);

            Assert.Null(userRet);
        }

        [Fact]
        public void Login_NotWrongPassword_ShouldReturnUser()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();


            VidconfileUser user = new VidconfileUser();

            string username = "test";
            string password = "nub";

            user.Username = username;

            userRepositoryMock.Setup(x => x.All())
                .Returns(new List<VidconfileUser>() { user }.AsQueryable());

            passwordHasherMock.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(true);

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            var userRet = userService.Login(username, password);

            passwordHasherMock.Verify(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);

            Assert.Same(user, userRet);
        }

        [Fact]
        public void Register_NullUsername_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string username = "test";
            string password = "nub";
            string profilePhotoUrl = "nubfas";

            string message = Assert.Throws<NullReferenceException>(() => userService.Register(null, password,profilePhotoUrl)).Message;

            Assert.Equal("username cannot be null", message);
        }

        [Fact]
        public void Register_NullPassword_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string username = "test";
            string password = "nub";
            string profilePhotoUrl = "nubfas";

            string message = Assert.Throws<NullReferenceException>(() => userService.Register(username, null, profilePhotoUrl)).Message;

            Assert.Equal("password cannot be null", message);
        }

        [Fact]
        public void Register_NullPhotoUrl_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string username = "test";
            string password = "nub";
            string profilePhotoUrl = "nubfas";

            string message = Assert.Throws<NullReferenceException>(() => userService.Register(username, password, null)).Message;

            Assert.Equal("profilePhotoUrl cannot be null", message);
        }

        [Fact]
        public void Register_ShouldAddUser()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            VidconfileUser user = null;

            userRepositoryMock.Setup(x => x.Add(It.IsAny<VidconfileUser>()))
                .Callback<VidconfileUser>(x => user = x);

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string username = "test";
            string password = "nub";
            string profilePhotoUrl = "nubfas";

            PasswordHashModel hashModel = new PasswordHashModel(new byte[1], new byte[2]);

            passwordHasherMock.Setup(x => x.CreatePasswordHash(password))
                .Returns(hashModel)
                .Verifiable();

            userService.Register(username, password, profilePhotoUrl);

            passwordHasherMock.Verify();
            unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
            userRepositoryMock.Verify(x => x.Add(It.IsAny<VidconfileUser>()), Times.Once);

            Assert.Equal(username, user.Username);
            Assert.Equal(profilePhotoUrl, user.ProfilePhotoUrl);
        }

        [Fact]
        public void SubscribeFromTo_NullFrom_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            VidconfileUser from = new VidconfileUser();
            VidconfileUser to = new VidconfileUser();

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string msg = Assert.Throws<NullReferenceException>(() => userService.SubscribeFromTo(null, to))
                .Message;

            Assert.Equal("from cannot be null", msg);
        }

        [Fact]
        public void SubscribeFromTo_NullTo_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            VidconfileUser from = new VidconfileUser();
            VidconfileUser to = new VidconfileUser();

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string msg = Assert.Throws<NullReferenceException>(() => userService.SubscribeFromTo(from, null))
                .Message;

            Assert.Equal("to cannot be null", msg);
        }

        [Fact]
        public void SubscribeFromTo_AlreadySubscribed_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            Guid fromId = Guid.NewGuid();
            Guid toId = Guid.NewGuid();

            VidconfileUser from = new VidconfileUser();
            VidconfileUser to = new VidconfileUser();

            from.Id = fromId;
            to.Id = toId;

            var sb = new SubscribeToSubscribers();

            sb.SubscribedToId = toId;
            sb.SubscriberId = fromId;

            subscribeToSubscriberMock.Setup(x => x.All())
                .Returns(new List<SubscribeToSubscribers>() { sb }.AsQueryable());

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string msg = Assert.Throws<ArgumentException>(() => userService.SubscribeFromTo(from, to))
                .Message;

            Assert.Equal("the user is already subscribed", msg);
        }

        [Fact]
        public void SubscribeFromTo_ShouldSubscribe()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            Guid fromId = Guid.NewGuid();
            Guid toId = Guid.NewGuid();

            VidconfileUser from = new VidconfileUser();
            VidconfileUser to = new VidconfileUser();

            from.Id = fromId;
            to.Id = toId;

            var sb = new SubscribeToSubscribers();

            sb.SubscribedToId = toId;
            sb.SubscriberId = Guid.Empty;

            SubscribeToSubscribers res = null;

            subscribeToSubscriberMock.Setup(x => x.All())
                .Returns(new List<SubscribeToSubscribers>() { sb }.AsQueryable());

            subscribeToSubscriberMock.Setup(x => x.Add(It.IsAny<SubscribeToSubscribers>()))
                .Callback<SubscribeToSubscribers>(x => res = x)
                .Verifiable();

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            userService.SubscribeFromTo(from, to);

            subscribeToSubscriberMock.Verify();
            unitOfWorkMock.Verify(x => x.Commit(), Times.Once);

            Assert.Equal(toId, res.SubscribedToId);
            Assert.Equal(fromId, res.SubscriberId);
        }

        [Fact]
        public void UnsubscribeFromTo_NullFrom_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            VidconfileUser from = new VidconfileUser();
            VidconfileUser to = new VidconfileUser();

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string msg = Assert.Throws<NullReferenceException>(() => userService.UnsubscribeFromTo(null, to))
                .Message;

            Assert.Equal("from cannot be null", msg);
        }

        [Fact]
        public void UnsubscribeFromTo_NullTo_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            VidconfileUser from = new VidconfileUser();
            VidconfileUser to = new VidconfileUser();

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string msg = Assert.Throws<NullReferenceException>(() => userService.UnsubscribeFromTo(from, null))
                .Message;

            Assert.Equal("to cannot be null", msg);
        }

        [Fact]
        public void UnsubscribeFromTo_AlreadySubscribed_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            Guid fromId = Guid.NewGuid();
            Guid toId = Guid.NewGuid();

            VidconfileUser from = new VidconfileUser();
            VidconfileUser to = new VidconfileUser();

            from.Id = fromId;
            to.Id = toId;

            var sb = new SubscribeToSubscribers();

            sb.SubscribedToId = toId;
            sb.SubscriberId = Guid.Empty;

            subscribeToSubscriberMock.Setup(x => x.All())
                .Returns(new List<SubscribeToSubscribers>() { sb }.AsQueryable());

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string msg = Assert.Throws<ArgumentException>(() => userService.UnsubscribeFromTo(from, to))
                .Message;

            Assert.Equal("the user is not subscribed", msg);
        }

        [Fact]
        public void UnsubscribeFromTo_ShouldUnsubscribe()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            Guid fromId = Guid.NewGuid();
            Guid toId = Guid.NewGuid();

            VidconfileUser from = new VidconfileUser();
            VidconfileUser to = new VidconfileUser();

            from.Id = fromId;
            to.Id = toId;

            var sb = new SubscribeToSubscribers();

            sb.SubscribedToId = toId;
            sb.SubscriberId = fromId;

            SubscribeToSubscribers res = null;

            subscribeToSubscriberMock.Setup(x => x.All())
                .Returns(new List<SubscribeToSubscribers>() { sb }.AsQueryable());

            subscribeToSubscriberMock.Setup(x => x.Delete(It.IsAny<SubscribeToSubscribers>()))
                .Callback<SubscribeToSubscribers>(x => res = x)
                .Verifiable();

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            userService.UnsubscribeFromTo(from, to);

            subscribeToSubscriberMock.Verify();
            unitOfWorkMock.Verify(x => x.Commit(), Times.Once);

            Assert.Same(sb, res);
        }

        [Fact]
        public void UserExist_NullUsername_ShoudThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            string username = "Ivan";

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string res = Assert.Throws<NullReferenceException>(() => userService.UserExists(null))
                .Message;

            Assert.Equal("username cannot be null or empty", res);
        }

        [Fact]
        public void UserExist_ShouldReturn()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            string username = "Ivan";

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            bool result = userService.UserExists(username);

            Assert.False(result);
        }

        [Fact]
        public void UserExist_ShouldReturnFalse()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            Guid user = Guid.NewGuid();

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            bool result = userService.UserExists(user);

            Assert.False(result);
        }

        [Fact]
        public void GetSubscriberCount_NullUser_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            VidconfileUser user = new VidconfileUser();

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string result = Assert.Throws<NullReferenceException>(() => userService.GetSubscriberCount(null))
                .Message;

            Assert.Equal("user cannot be null", result);
        }

        [Fact]
        public void GetSubscriberCount_ShouldGet()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            VidconfileUser user = new VidconfileUser();

            Guid userId = Guid.NewGuid();
            user.Id = userId;

            subscribeToSubscriberMock.Setup(x => x.All())
                .Returns(new List<SubscribeToSubscribers>() { new SubscribeToSubscribers() { SubscribedToId = userId },
                new SubscribeToSubscribers() { SubscribedToId = userId }, new SubscribeToSubscribers() { SubscribedToId = userId }}.AsQueryable());

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            int subs = userService.GetSubscriberCount(user);

            Assert.Equal(3, subs);
        }

        [Fact]
        public void GetUserById_ShouldGet()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();
            
            Guid userId = Guid.NewGuid();

            subscribeToSubscriberMock.Setup(x => x.All())
                .Returns(new List<SubscribeToSubscribers>() { new SubscribeToSubscribers() { SubscribedToId = userId },
                new SubscribeToSubscribers() { SubscribedToId = userId }, new SubscribeToSubscribers() { SubscribedToId = userId }}.AsQueryable());

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            var userNew = userService.GetUserById(userId);

            userRepositoryMock.Verify(x => x.GetById(userId), Times.Once);
        }

        [Fact]
        public void GetUserByIdWithVideos_ShouldGet()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            Guid userId = Guid.NewGuid();

            subscribeToSubscriberMock.Setup(x => x.All())
                .Returns(new List<SubscribeToSubscribers>() { new SubscribeToSubscribers() { SubscribedToId = userId },
                new SubscribeToSubscribers() { SubscribedToId = userId }, new SubscribeToSubscribers() { SubscribedToId = userId }}.AsQueryable());

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            var userNew = userService.GetUserByIdWithVideos(userId);

            userRepositoryMock.Verify(x => x.All("UploadedVideos"), Times.Once);
        }

        [Fact]
        public void GetAllUsers_ShouldGet()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            Guid userId = Guid.NewGuid();

            subscribeToSubscriberMock.Setup(x => x.All())
                .Returns(new List<SubscribeToSubscribers>() { new SubscribeToSubscribers() { SubscribedToId = userId },
                new SubscribeToSubscribers() { SubscribedToId = userId }, new SubscribeToSubscribers() { SubscribedToId = userId }}.AsQueryable());

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            var userNew = userService.GetAllUsers();

            userRepositoryMock.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public void EditProfile_UserNull_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            Guid userId = Guid.NewGuid();

            VidconfileUser user = new VidconfileUser();

            string photoUrl = "asd";

            subscribeToSubscriberMock.Setup(x => x.All())
                .Returns(new List<SubscribeToSubscribers>() { new SubscribeToSubscribers() { SubscribedToId = userId },
                new SubscribeToSubscribers() { SubscribedToId = userId }, new SubscribeToSubscribers() { SubscribedToId = userId }}.AsQueryable());

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string msg = Assert.Throws<NullReferenceException>(() => userService.EditProfile(null, photoUrl))
                .Message;

            Assert.Equal("user cannot be null", msg);
        }

        [Fact]
        public void EditProfile_PhotoNull_ShouldThrow()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            Guid userId = Guid.NewGuid();

            VidconfileUser user = new VidconfileUser();

            string photoUrl = "asd";

            subscribeToSubscriberMock.Setup(x => x.All())
                .Returns(new List<SubscribeToSubscribers>() { new SubscribeToSubscribers() { SubscribedToId = userId },
                new SubscribeToSubscribers() { SubscribedToId = userId }, new SubscribeToSubscribers() { SubscribedToId = userId }}.AsQueryable());

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            string msg = Assert.Throws<NullReferenceException>(() => userService.EditProfile(user, null))
                .Message;

            Assert.Equal("newProfilePhotoUrl cannot be null", msg);
        }

        [Fact]
        public void EditProfile_ShouldUpdate()
        {
            var videoRepositoryMock = new Mock<IRepository<Video>>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var userRepositoryMock = new Mock<IRepository<VidconfileUser>>();
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var subscribeToSubscriberMock = new Mock<IRepository<SubscribeToSubscribers>>();

            Guid userId = Guid.NewGuid();

            VidconfileUser user = new VidconfileUser();

            string photoUrl = "asd";

            subscribeToSubscriberMock.Setup(x => x.All())
                .Returns(new List<SubscribeToSubscribers>() { new SubscribeToSubscribers() { SubscribedToId = userId },
                new SubscribeToSubscribers() { SubscribedToId = userId }, new SubscribeToSubscribers() { SubscribedToId = userId }}.AsQueryable());

            VidconfileUserServices userService =
                new VidconfileUserServices(userRepositoryMock.Object, unitOfWorkMock.Object, passwordHasherMock.Object,
                videoRepositoryMock.Object, subscribeToSubscriberMock.Object);

            userService.EditProfile(user, photoUrl);

            Assert.Equal(photoUrl, user.ProfilePhotoUrl);
            userRepositoryMock.Verify(x => x.Update(user), Times.Once);
            unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
        }
    }
}
