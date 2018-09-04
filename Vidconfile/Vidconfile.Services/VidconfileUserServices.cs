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

        public VidconfileUserServices(IRepository<VidconfileUser> userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            this.userRepository = userRepository ?? throw new NullReferenceException("userRepository cannot be null");
            this.unitOfWork = unitOfWork ?? throw new NullReferenceException("unitOfWork cannot be null");
            this.passwordHasher = passwordHasher ?? throw new NullReferenceException("passwordHasher cannot be null");
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

        public VidconfileUser Register(VidconfileUser user, string password)
        {
            if (user == null)
            {
                throw new NullReferenceException("user cannot be null");
            }

            if (password == null)
            {
                throw new NullReferenceException("password cannot be null");
            }

            PasswordHashModel hashModel = this.passwordHasher.CreatePasswordHash(password);

            user.PasswordHash = hashModel.PasswordHash;
            user.PasswordSalt = hashModel.PasswordSalt;

            this.userRepository.Add(user);

            this.unitOfWork.Commit();

            return user;
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
    }
}
