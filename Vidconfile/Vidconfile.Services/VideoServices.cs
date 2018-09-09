using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vidconfile.Data.Contracts;
using Vidconfile.Data.Models;
using Vidconfile.Services.Services;

namespace Vidconfile.Services
{
    public class VideoServices : IVideoServices
    {
        private IRepository<Video> videoRepository;
        private IUnitOfWork unitOfWork;

        public VideoServices(IRepository<Video> videoRepository, IUnitOfWork unitOfWork)
        {
            this.videoRepository = videoRepository ?? throw new NullReferenceException("videoRepository cannot be null");
            this.unitOfWork = unitOfWork ?? throw new NullReferenceException("unitOfWork cannot be null");
        }

        public IQueryable<Video> GetAllVideos()
        {
            return this.videoRepository.All();
        }

        public Video GetVideoById(Guid id)
        {
            return this.videoRepository.All().FirstOrDefault(x => x.Id == id);
        }

        public void UploadVideo(Guid uploaderId, byte[] videoData, string description, string thumbnailUrl, string title)
        {
            if (videoData == null)
            {
                throw new NullReferenceException("videoData cannot be null");
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new NullReferenceException("description cannot be null");
            }

            if (string.IsNullOrEmpty(thumbnailUrl))
            {
                throw new NullReferenceException("thumbnailUrl cannot be null");
            }

            if (string.IsNullOrEmpty(title))
            {
                throw new NullReferenceException("title cannot be null");
            }

            Video video = new Video();

            video.Title = title;
            video.ThumbnailUrl = thumbnailUrl;
            video.Description = description;
            video.VideoData = videoData;
            video.UploaderId = uploaderId;
            video.Created = DateTime.Now;
            video.LikeCount = 0;

            this.videoRepository.Add(video);

            this.unitOfWork.Commit();
        }
    }
}
