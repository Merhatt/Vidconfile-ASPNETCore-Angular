using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Vidconfile.Data.Models
{
    public class VidconfileUser : BaseDBModel
    {
        private ICollection<Video> uploadedVideos;
        private ICollection<Comment> comments;

        public VidconfileUser()
        {
            this.uploadedVideos = new HashSet<Video>();
            this.comments = new HashSet<Comment>();
        }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt{ get; set; }

        public string ProfilePhotoUrl { get; set; }

        public virtual ICollection<Video> UploadedVideos { get { return this.uploadedVideos; } set { this.uploadedVideos = value; } }

        public virtual ICollection<Comment> Comments { get { return this.comments; } set { this.comments = value; } }
    }
}
