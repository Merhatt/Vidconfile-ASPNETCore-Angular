using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vidconfile.Constants;

namespace Vidconfile.ApiModels
{
    public class AddCommentApiModel
    {
        public Guid VideoId { get; set; }

        [Required]
        [StringLength(UserConstants.MaxCommentLength, MinimumLength = UserConstants.MinCommentLength, ErrorMessage = "Your comment length is invalid")]
        public string CommentText { get; set; }
    }
}
