using System;
using Vidconfile.Utils.Models;
using Xunit;

namespace Vidconfile.Utils.Tests
{
    public class PasswordHasherTests
    {
        [Fact]
        public void CreatePasswordHash_NullPassword_ShouldThrow()
        {
            PasswordHasher hasher = new PasswordHasher();

            string msg = Assert.Throws<NullReferenceException>(() => hasher.CreatePasswordHash(null))
                .Message;

            Assert.Equal("password cannot be null or empty", msg);
        }

        [Fact]
        public void CreatePasswordHash_ShouldCreate()
        {
            PasswordHasher hasher = new PasswordHasher();

            string password = "secret";

            PasswordHashModel model = hasher.CreatePasswordHash(password);

            Assert.NotNull(model);
            Assert.NotNull(model.PasswordHash);
            Assert.NotNull(model.PasswordSalt);
        }

        [Fact]
        public void VerifyPasswordHash_NullPassword_ShouldThrow()
        {
            PasswordHasher hasher = new PasswordHasher();

            string password = "secret";
            byte[] passwordHash = new byte[3]; 
            byte[] passwordSalt = new byte[4];

            string msg = Assert.Throws<NullReferenceException>(() => hasher.VerifyPasswordHash(null, passwordHash, passwordSalt))
                .Message;

            Assert.Equal("password cannot be null or empty", msg);
        }

        [Fact]
        public void VerifyPasswordHash_NullHash_ShouldThrow()
        {
            PasswordHasher hasher = new PasswordHasher();

            string password = "secret";
            byte[] passwordHash = new byte[3];
            byte[] passwordSalt = new byte[4];

            string msg = Assert.Throws<NullReferenceException>(() => hasher.VerifyPasswordHash(password, null, passwordSalt))
                .Message;

            Assert.Equal("passwordHash cannot be null or empty", msg);
        }

        [Fact]
        public void VerifyPasswordHash_NullSakt_ShouldThrow()
        {
            PasswordHasher hasher = new PasswordHasher();

            string password = "secret";
            byte[] passwordHash = new byte[3];
            byte[] passwordSalt = new byte[4];

            string msg = Assert.Throws<NullReferenceException>(() => hasher.VerifyPasswordHash(password, passwordHash, null))
                .Message;

            Assert.Equal("passwordSalt cannot be null or empty", msg);
        }

        [Fact]
        public void VerifyPasswordHash_CorrectHash_ShouldReturnTrue()
        {
            PasswordHasher hasher = new PasswordHasher();

            string password = "secret";

            var model = hasher.CreatePasswordHash(password);

            bool isCorrect = hasher.VerifyPasswordHash(password, model.PasswordHash, model.PasswordSalt);

            Assert.True(isCorrect);
        }

        [Fact]
        public void VerifyPasswordHash_WrongHash_ShouldReturnFalse()
        {
            PasswordHasher hasher = new PasswordHasher();

            string password = "secret";

            var model = hasher.CreatePasswordHash(password);

            string wrongPassword = "nicetry";

            bool isCorrect = hasher.VerifyPasswordHash(wrongPassword, model.PasswordHash, model.PasswordSalt);

            Assert.False(isCorrect);
        }
    }
}
