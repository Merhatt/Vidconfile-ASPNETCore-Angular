using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private IRepository<Comment> commentRepository;

        public VideoServices(IRepository<Video> videoRepository, IUnitOfWork unitOfWork,
            IRepository<Comment> commentRepository)
        {
            this.videoRepository = videoRepository ?? throw new NullReferenceException("videoRepository cannot be null");
            this.unitOfWork = unitOfWork ?? throw new NullReferenceException("unitOfWork cannot be null");
            this.commentRepository = commentRepository ?? throw new NullReferenceException("commentRepository cannot be null");
        }

        public void ChangeViewsOfVideo(Video video, ulong views)
        {
            if (video == null)
            {
                throw new NullReferenceException("video cannot be null");
            }

            video.Views = views;

            this.videoRepository.Update(video);

            this.unitOfWork.Commit();
        }

        public IQueryable<Video> GetAllVideos()
        {
            return this.videoRepository.All();
        }

        public Video GetVideoById(Guid id)
        {
            return this.videoRepository.GetById(id);
        }

        public Video GetVideoByIdWithComments(Guid id)
        {
            return this.videoRepository.All("Comments.Author")
                .FirstOrDefault(x => x.Id == id);
        }

        public void UploadVideo(VidconfileUser uploader, byte[] videoData, string description, string thumbnailUrl, string title)
        {
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

            video.Uploader = uploader ?? throw new NullReferenceException("uploader cannot be null");
            video.Title = title;
            video.ThumbnailUrl = thumbnailUrl;
            video.Description = description;
            video.VideoData = videoData ?? throw new NullReferenceException("videoData cannot be null");
            video.Created = DateTime.Now;

            this.videoRepository.Add(video);

            this.unitOfWork.Commit();
        }
    }
}
