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

        public VideoServices(IRepository<Video> videoRepository)
        {
            this.videoRepository = videoRepository ?? throw new NullReferenceException("videoRepository cannot be null");
        }

        public IQueryable<Video> GetAllVideos()
        {
            return this.videoRepository.All();
        }
    }
}
