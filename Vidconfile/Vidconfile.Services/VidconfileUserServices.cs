using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vidconfile.Data.Contracts;
using Vidconfile.Data.Models;
using Vidconfile.Services.Services;
using Vidconfile.Utils.Contracts;
using Vidconfile.Utils.Models;

namespace Vidconfile.Services
{
    public class VidconfileUserServices : IVidconfileUserServices
    {
        private IRepository<VidconfileUser> userRepository;
        private IUnitOfWork unitOfWork;
        private IPasswordHasher passwordHasher;
        private IRepository<Video> videoRepository;
        private IRepository<SubscribeToSubscribers> subscribeToSubscribersRepository;

        public VidconfileUserServices(IRepository<VidconfileUser> userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher,
            IRepository<Video> videoRepository, IRepository<SubscribeToSubscribers> subscribeToSubscribersRepository)
        {
            this.userRepository = userRepository ?? throw new NullReferenceException("userRepository cannot be null");
            this.unitOfWork = unitOfWork ?? throw new NullReferenceException("unitOfWork cannot be null");
            this.passwordHasher = passwordHasher ?? throw new NullReferenceException("passwordHasher cannot be null");
            this.videoRepository = videoRepository ?? throw new NullReferenceException("videoRepository cannot be null");
            this.subscribeToSubscribersRepository = subscribeToSubscribersRepository ?? throw new NullReferenceException("subscribeToSubscribersRepository cannot be null");
        }

        public VidconfileUser GetUserByVideoId(Guid videoId)
        {
            Video video = this.videoRepository.All(x => x.Uploader)
                .FirstOrDefault(x => x.Id == videoId);

            if (video == null)
            {
                return null;
            }

            VidconfileUser user = video.Uploader;

            return user;
        }

        public VidconfileUser Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new NullReferenceException("username cannot be null or empty");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new NullReferenceException("password cannot be null or empty");
            }

            VidconfileUser user = this.userRepository
                .All()
                .FirstOrDefault(x => x.Username == username);

            if (user == null)
            {
                return null;
            }

            if (this.passwordHasher.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) == false)
            {
                return null;
            }

            return user;
        }

        public void Register(string username, string password, string profilePhotoUrl)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new NullReferenceException("username cannot be null");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new NullReferenceException("password cannot be null");
            }

            if (string.IsNullOrEmpty(profilePhotoUrl))
            {
                throw new NullReferenceException("profilePhotoUrl cannot be null");
            }

            PasswordHashModel hashModel = this.passwordHasher.CreatePasswordHash(password);

            var user = new VidconfileUser();

            user.PasswordHash = hashModel.PasswordHash;
            user.PasswordSalt = hashModel.PasswordSalt;

            user.Username = username;
            user.ProfilePhotoUrl = profilePhotoUrl;

            this.userRepository.Add(user);

            this.unitOfWork.Commit();
        }

        public void SubscribeFromTo(VidconfileUser from, VidconfileUser to)
        {
            if (from == null)
            {
                throw new NullReferenceException("from cannot be null");
            }

            if (to == null)
            {
                throw new NullReferenceException("to cannot be null");
            }

            if (this.IsSubscribed(from, to))
            {
                throw new ArgumentException("the user is already subscribed");
            }

            var subscriberToSubscribers = new SubscribeToSubscribers();
            subscriberToSubscribers.SubscriberId = from.Id;
            
            subscriberToSubscribers.SubscribedToId = to.Id;

            this.subscribeToSubscribersRepository.Add(subscriberToSubscribers);

            this.unitOfWork.Commit();
        }

        public void UnsubscribeFromTo(VidconfileUser from, VidconfileUser to)
        {
            if (from == null)
            {
                throw new NullReferenceException("from cannot be null");
            }

            if (to == null)
            {
                throw new NullReferenceException("to cannot be null");
            }

            if (!this.IsSubscribed(from, to))
            {
                throw new ArgumentException("the user is not subscribed");
            }

            var subToDelete = this.subscribeToSubscribersRepository.All()
                .FirstOrDefault(x => x.SubscribedToId == to.Id && x.SubscriberId == from.Id);

            this.subscribeToSubscribersRepository.Delete(subToDelete);

            this.unitOfWork.Commit();
        }

        public bool UserExists(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new NullReferenceException("username cannot be null or empty");
            }

            bool userExists = this.userRepository.All().Any(x => x.Username == username);

            return userExists;
        }

        public bool UserExists(Guid id)
        {
            bool userExists = this.userRepository.All().Any(x => x.Id == id);

            return userExists;
        }

        public int GetSubscriberCount(VidconfileUser user)
        {
            if (user == null)
            {
                throw new NullReferenceException("user cannot be null");
            }

            var count = this.subscribeToSubscribersRepository.All()
                .Where(x => x.SubscribedToId == user.Id).Count();

            return count;
        }

        public bool IsSubscribed(VidconfileUser from, VidconfileUser to)
        {
            if (from == null)
            {
                throw new NullReferenceException("from cannot be null");
            }

            if (to == null)
            {
                throw new NullReferenceException("to cannot be null");
            }

            bool isSubscribed = this.subscribeToSubscribersRepository.All()
                .FirstOrDefault(x => x.SubscribedToId == to.Id && x.SubscriberId == from.Id) != null;

            return isSubscribed;
        }

        public VidconfileUser GetUserById(Guid id)
        {
            return this.userRepository.GetById(id);
        }

        public VidconfileUser GetUserByIdWithVideos(Guid id)
        {
            return this.userRepository.All("UploadedVideos")
                .FirstOrDefault(x => x.Id == id);
        }

        public void EditProfile(VidconfileUser user, string newProfilePhotoUrl)
        {
            if (user == null)
            {
                throw new NullReferenceException("user cannot be null");
            }

            if (string.IsNullOrEmpty(newProfilePhotoUrl))
            {
                throw new NullReferenceException("newProfilePhotoUrl cannot be null");
            }

            user.ProfilePhotoUrl = newProfilePhotoUrl;

            this.userRepository.Update(user);

            this.unitOfWork.Commit();
        }

        public IQueryable<VidconfileUser> GetAllUsers()
        {
            return this.userRepository.All();
        }
    }
}
