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
        
        public string CommentText { get; set; }

        public Video Video { get; set; }

        public VidconfileUser Author { get; set; }
    }
}
