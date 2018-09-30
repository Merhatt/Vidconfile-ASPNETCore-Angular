using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vidconfile.ApiModels
{
    public class GetAllVideoApiModel
    {
        public Guid Id { get; set; }

        public Guid UploaderId { get; set; }

        public string ThumbnailUrl { get; set; }

        public ulong Views { get; set; }

        public string Title { get; set; }

        public DateTime Created { get; set; }
    }
}
