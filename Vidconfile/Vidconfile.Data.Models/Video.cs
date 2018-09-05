using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vidconfile.Data.Models
{
    public class Video : BaseDBModel
    {
        private ICollection<VidconfileUserVideo> likedUsers;

        public Video()
        {
            this.likedUsers = new HashSet<VidconfileUserVideo>();
        }

        [Required]
        public Guid UploaderId { get; set; }
        
        public virtual VidconfileUser Uploader { get; set; }

        public virtual ICollection<VidconfileUserVideo> LikedUsers { get { return this.likedUsers; } set { this.likedUsers = value; } }
    }
}