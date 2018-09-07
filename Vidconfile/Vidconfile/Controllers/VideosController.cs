using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vidconfile.ApiModels;
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
        public IActionResult UploadVideo()
        {

        }
    }
}