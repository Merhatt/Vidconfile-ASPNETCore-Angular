using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Vidconfile.ApiModels;
using Vidconfile.Constants;
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
        private readonly IMemoryCache cache;
        private readonly ICommentServices commentServices;
        private readonly IVidconfileUserServices userServices;

        public VideosController(IVideoServices videoServices, IMapper mapper, IMemoryCache cache,
            ICommentServices commentServices, IVidconfileUserServices userServices)
        {
            this.videoServices = videoServices ?? throw new NullReferenceException("videoServices cannot be null");
            this.mapper = mapper ?? throw new NullReferenceException("mapper cannot be null");
            this.cache = cache ?? throw new NullReferenceException("cache cannot be null");
            this.commentServices = commentServices ?? throw new NullReferenceException("commentServices cannot be null");
            this.userServices = userServices ?? throw new NullReferenceException("userServices cannot be null");
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

            var user = this.userServices.GetUserById(cl);

            if (user == null)
            {
                return Unauthorized();
            }

            if (Request.Form.Files.Count == 0)
            {
                return BadRequest("Video is missing");
            }

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

            this.videoServices.UploadVideo(user, videoData, description, thumbnailUrl, title);

            return Ok();
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(Guid id)
        {
            Video video = this.videoServices.GetVideoById(id);

            if (video == null)
            {
                return NotFound();
            }

            GetVideoByIdApiModel model = this.mapper.Map<GetVideoByIdApiModel>(video);

            string viewsCacheKey = string.Format(VideoConstants.MemoryCacheViewsTemplate, model.Id);

            ulong viewsSinceLastCache = this.cache.GetOrCreate<ulong>(viewsCacheKey, entry =>
            {
                return 0;
            });

            viewsSinceLastCache++;

            this.cache.Set<ulong>(viewsCacheKey, viewsSinceLastCache);

            model.Views += viewsSinceLastCache;

            if (viewsSinceLastCache >= VideoConstants.ViewsPerToCache)
            {
                this.cache.Remove(viewsCacheKey);

                this.videoServices.ChangeViewsOfVideo(video, video.Views + viewsSinceLastCache);
            }

            return Ok(model);
        }

        [HttpGet("getvideobyid")]
        public FileContentResult GetVideoById(Guid id)
        {
            Video video = this.videoServices.GetVideoById(id);

            return File(video.VideoData, "video/mp4", "video.mp4");
        }

        [Authorize]
        [HttpPost("addcomment")]
        public IActionResult AddComment(AddCommentApiModel commentModel)
        {
            Guid cl = Guid.Parse(this.User.Claims
                .FirstOrDefault(x => !x.Properties.FirstOrDefault(v => v.Value == "nameid").Equals(default(KeyValuePair<string, string>)))
                .Value);

            if (string.IsNullOrEmpty(commentModel.CommentText))
            {
                return NotFound("Comment can't be empty");
            }

            VidconfileUser user = this.userServices.GetUserById(cl);

            if (user == null)
            {
                return Unauthorized();
            }

            Video video = this.videoServices.GetVideoById(commentModel.VideoId);

            if (video == null)
            {
                return NotFound("Video is not found");
            }

            var commentRes = this.commentServices.AddComment(video, user, commentModel.CommentText);

            GetAllCommentsApiModel model = this.mapper.Map<GetAllCommentsApiModel>(commentRes);

            return Ok(model);
        }

        [HttpGet("getallcomments")]
        public IActionResult GetAllComments(Guid videoId)
        {
            var video = this.videoServices.GetVideoByIdWithComments(videoId);

            if (video == null)
            {
                return NotFound("Video is not found");
            }

            var comments = video.Comments.ToList();

            IEnumerable<GetAllCommentsApiModel> model = this.mapper.Map<IEnumerable<GetAllCommentsApiModel>>(comments);

            return Ok(model);
        }
    }
}