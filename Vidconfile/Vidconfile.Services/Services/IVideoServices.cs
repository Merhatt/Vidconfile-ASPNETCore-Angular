using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vidconfile.Data.Models;

namespace Vidconfile.Services.Services
{
    public interface IVideoServices
    {
        IQueryable<Video> GetAllVideos();

        void UploadVideo(VidconfileUser uploader, byte[] videoData, string description, string thumbnailUrl, string title);

        Video GetVideoById(Guid id);

        Video GetVideoByIdWithComments(Guid id);

        void ChangeViewsOfVideo(Video video, ulong views);
    }
}
