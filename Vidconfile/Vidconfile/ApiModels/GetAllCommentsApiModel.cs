using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vidconfile.ApiModels
{
    public class GetAllCommentsApiModel
    {
        public Guid Id { get; set; }

        public Guid VideoId { get; set; }

        public Guid AuthorId { get; set; }

        public DateTime Created { get; set; }

        public string CommentText { get; set; }

        public string AuthorUsername { get; set; }

        public string AuthorProfilePhotoUrl { get; set; }
    }
}
