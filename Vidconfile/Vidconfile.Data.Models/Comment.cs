using System;
using System.Collections.Generic;
using System.Text;

namespace Vidconfile.Data.Models
{
    public class Comment : BaseDBModel
    {
        public Comment()
        {
        }

        public long Likes { get; set; }

        public Guid VideoId { get; set; }

        public virtual Video Video { get; set; }

        public Guid AuthorId { get; set; }

        public virtual VidconfileUser Author { get; set; }
    }
}
