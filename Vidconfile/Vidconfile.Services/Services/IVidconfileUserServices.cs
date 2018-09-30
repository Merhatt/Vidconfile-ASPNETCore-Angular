using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vidconfile.Data.Models;

namespace Vidconfile.Services.Services
{
    public interface IVidconfileUserServices
    {
        void Register(string username, string password, string profilePhotoUrl);

        VidconfileUser Login(string username, string password);

        VidconfileUser GetUserByVideoId(Guid videoId);

        VidconfileUser GetUserById(Guid id);

        VidconfileUser GetUserByIdWithVideos(Guid id);

        IQueryable<VidconfileUser> GetAllUsers();

        bool UserExists(string username);

        bool UserExists(Guid id);

        bool IsSubscribed(VidconfileUser from, VidconfileUser to);

        void SubscribeFromTo(VidconfileUser from, VidconfileUser to);

        void UnsubscribeFromTo(VidconfileUser from, VidconfileUser to);

        void EditProfile(VidconfileUser user, string newProfilePhotoUrl);

        int GetSubscriberCount(VidconfileUser user);
    }
}
