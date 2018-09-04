using System;
using System.Collections.Generic;
using System.Text;

namespace Vidconfile.Utils.Models
{
    public class PasswordHashModel
    {
        public PasswordHashModel(byte[] passwordHash, byte[] passwordSalt)
        {
            this.PasswordHash = passwordHash ?? throw new NullReferenceException("passwordHash cannot be null");
            this.PasswordSalt = passwordSalt ?? throw new NullReferenceException("passwordSalt cannot be null");
        }

        public byte[] PasswordHash { get; private set; }

        public byte[] PasswordSalt { get; private set; }
    }
}
