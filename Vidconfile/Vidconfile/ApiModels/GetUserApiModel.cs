using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vidconfile.ApiModels
{
    public class GetUserApiModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string ProfilePhotoUrl { get; set; }

        public int SubscriberCount { get; set; }

        public ICollection<GetVideoByIdApiModel> Videos { get; set; }
    }
}
