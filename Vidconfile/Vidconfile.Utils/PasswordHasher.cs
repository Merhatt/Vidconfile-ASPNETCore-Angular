using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Vidconfile.Utils.Contracts;
using Vidconfile.Utils.Models;

namespace Vidconfile.Utils
{
    public class PasswordHasher : IPasswordHasher   
    {
        public PasswordHashModel CreatePasswordHash(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new NullReferenceException("password cannot be null or empty");
            }

            PasswordHashModel hashModel;

            using (var hmac = new HMACSHA512())
            {
                hashModel = new PasswordHashModel(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)), hmac.Key);
            }

            return hashModel;
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new NullReferenceException("password cannot be null or empty");
            }

            if (passwordHash == null)
            {
                throw new NullReferenceException("passwordHash cannot be null or empty");
            }

            if (passwordSalt == null)
            {
                throw new NullReferenceException("passwordSalt cannot be null or empty");
            }

            byte[] computedHash;
        
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordHash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
