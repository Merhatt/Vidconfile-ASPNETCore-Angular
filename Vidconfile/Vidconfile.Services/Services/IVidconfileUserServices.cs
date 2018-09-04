using System;
using System.Collections.Generic;
using System.Text;
using Vidconfile.Data.Models;

namespace Vidconfile.Services.Services
{
    public interface IVidconfileUserServices
    {
        VidconfileUser Register(VidconfileUser user, string password);

        VidconfileUser Login(string username, string password);

        bool UserExists(string username);
    }
}
