using System;
using System.Collections.Generic;
using System.Text;
using Vidconfile.Utils.Models;

namespace Vidconfile.Utils.Contracts
{
    public interface IPasswordHasher
    {
        PasswordHashModel CreatePasswordHash(string password);

        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
