using System;
using System.Collections.Generic;
using System.Text;

namespace Vidconfile.Data.Models
{
    public class VidconfileUser
    {
        public Guid? Id { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt{ get; set; }
    }
}
