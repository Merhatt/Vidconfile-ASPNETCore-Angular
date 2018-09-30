using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vidconfile.Data.Models
{
    public class Video : BaseDBModel
    {
        private ICollection<Comment> comments;

        public Video()
        {
            this.comments = new HashSet<Comment>();
        }

        public string ThumbnailUrl { get; set; }

        public ulong Views { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public byte[] VideoData { get; set; }

        public virtual VidconfileUser Uploader { get; set; }

        public ICollection<Comment> Comments { get { return this.comments; } set { this.comments = value; } }
    }
}