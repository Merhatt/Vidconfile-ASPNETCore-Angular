using System;
using System.Collections.Generic;
using System.Text;
using Vidconfile.Data.Models;

namespace Vidconfile.Services.Services
{
    public interface ICommentServices
    {
        Comment AddComment(Video video, VidconfileUser user, string commentText);
    }
}
