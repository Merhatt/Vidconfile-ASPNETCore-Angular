using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vidconfile.ApiModels;
using Vidconfile.Data.Models;
using Vidconfile.Services.Services;

namespace Vidconfile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IVideoServices videoServices;
        private readonly IMapper mapper;

        public VideosController(IVideoServices videoServices, IMapper mapper)
        {
            this.videoServices = videoServices ?? throw new NullReferenceException("videoServices cannot be null");
            this.mapper = mapper ?? throw new NullReferenceException("mapper cannot be null");
        }

        [HttpGet]
        public IActionResult Get()
        {
            var allVideos = this.videoServices.GetAllVideos()
                .ToList();

            var allReturnableVideos = this.mapper.Map<IEnumerable<GetAllVideoApiModel>>(allVideos);

            return Ok(allReturnableVideos);
        }

        [HttpPost("uploadvideo")]
        [Authorize]
        public IActionResult UploadVideo()
        {
            Guid cl = Guid.Parse(this.User.Claims
                .FirstOrDefault(x => !x.Properties.FirstOrDefault(v => v.Value == "nameid").Equals(default(KeyValuePair<string, string>)))
                .Value);

            var file = Request.Form.Files[0];

            byte[] fileBytes;

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            var videoData = fileBytes;
            var description = this.Request.Form["description"];
            var thumbnailUrl = this.Request.Form["thumbnailUrl"];
            var title = this.Request.Form["title"];

            this.videoServices.UploadVideo(cl, videoData, description, thumbnailUrl, title);

            return Ok();
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(Guid id)
        {
            Video video = this.videoServices.GetVideoById(id);

            GetVideoByIdApiModel model = this.mapper.Map<GetVideoByIdApiModel>(video);

            return Ok(model);
        }

        [HttpGet("getvideobyid")]
        public FileContentResult GetVideoById(Guid id)
        {
            Video video = this.videoServices.GetVideoById(id);

            return File(video.VideoData, "video/mp4", "video.mp4");
        }
    }
}