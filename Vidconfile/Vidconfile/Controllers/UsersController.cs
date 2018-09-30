using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vidconfile.ApiModels;
using Vidconfile.Data.Models;
using Vidconfile.Services;
using Vidconfile.Services.Services;

namespace Vidconfile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IVidconfileUserServices userServices;
        private readonly IMapper mapper;

        public UsersController(IVidconfileUserServices userServices, IMapper mapper)
        {
            this.userServices = userServices ?? throw new NullReferenceException("userServices cannot be null");
            this.mapper = mapper ?? throw new NullReferenceException("mapper cannot be null");
        }

        [HttpGet]
        public IActionResult Get()
        {
            var allUsers = this.userServices.GetAllUsers()
                .ToList();

            ICollection<GetAllUsersApiModel> users = new HashSet<GetAllUsersApiModel>();

            foreach (var item in allUsers)
            {
                var userToAdd = this.mapper.Map<GetAllUsersApiModel>(item);

                userToAdd.SubscriberCount = this.userServices.GetSubscriberCount(item);

                users.Add(userToAdd);
            }

            return Ok(users);
        }

        [HttpGet("getbyvideoid")]
        public IActionResult GetByVideoId(Guid id)
        {
            var user = this.userServices.GetUserByVideoId(id);

            if (user == null)
            {
                return NotFound("Coud not find the uploader of this video");
            }

            var model = this.mapper.Map<GetUserByVideoIdApiModel>(user);

            model.SubscriberCount = this.userServices.GetSubscriberCount(user);

            return Ok(model);
        }

        [Authorize]
        [HttpGet("subscribe")]
        public IActionResult Subscribe(Guid userToSubscribeTo)
        {
            Guid cl = Guid.Parse(this.User.Claims
                .FirstOrDefault(x => !x.Properties.FirstOrDefault(v => v.Value == "nameid").Equals(default(KeyValuePair<string, string>)))
                .Value);

            VidconfileUser userFrom = this.userServices.GetUserById(cl);
            VidconfileUser userTo = this.userServices.GetUserById(userToSubscribeTo);

            if (userFrom == null || userTo == null)
            {
                return BadRequest("User to subscribe to does not exist");
            }

            if (this.userServices.IsSubscribed(userFrom, userTo))
            {
                return BadRequest("You are already subscribed to this user");
            }

            this.userServices.SubscribeFromTo(userFrom, userTo);

            return Ok();
        }

        [Authorize]
        [HttpGet("unsubscribe")]
        public IActionResult Unsubscribe(Guid userToUnsubscribeTo)
        {
            Guid cl = Guid.Parse(this.User.Claims
                .FirstOrDefault(x => !x.Properties.FirstOrDefault(v => v.Value == "nameid").Equals(default(KeyValuePair<string, string>)))
                .Value);

            VidconfileUser userFrom = this.userServices.GetUserById(cl);
            VidconfileUser userTo = this.userServices.GetUserById(userToUnsubscribeTo);

            if (userFrom == null || userTo == null)
            {
                return BadRequest("User to unsubscribe to does not exist");
            }

            if (!this.userServices.IsSubscribed(userFrom, userTo))
            {
                return BadRequest("You are not subscribed to this user");
            }

            this.userServices.UnsubscribeFromTo(userFrom, userTo);

            return Ok();
        }

        [Authorize]
        [HttpGet("issubscribed")]
        public IActionResult IsSubscribed(Guid isSubscribedUser)
        {
            Guid cl = Guid.Parse(this.User.Claims
                .FirstOrDefault(x => !x.Properties.FirstOrDefault(v => v.Value == "nameid").Equals(default(KeyValuePair<string, string>)))
                .Value);

            VidconfileUser userFrom = this.userServices.GetUserById(cl);
            VidconfileUser userTo = this.userServices.GetUserById(isSubscribedUser);

            if (userFrom == null || userTo == null)
            {
                return BadRequest("User to unsubscribe to does not exist");
            }


            bool isSubscribed = this.userServices.IsSubscribed(userFrom, userTo);

            return Ok(new
            {
                isSubscribed
            });
        }

        [Authorize]
        [HttpPost("edit")]
        public IActionResult EditProfile(EditProfileApiModel model)
        {
            Guid cl = Guid.Parse(this.User.Claims
                .FirstOrDefault(x => !x.Properties.FirstOrDefault(v => v.Value == "nameid").Equals(default(KeyValuePair<string, string>)))
                .Value);

            VidconfileUser user = this.userServices.GetUserById(cl);

            if (user == null)
            {
                return BadRequest("User does not exist");
            }

            this.userServices.EditProfile(user, model.AuthorProfilePhotoUrl);

            return Ok();
        }

        [HttpGet("getuser")]
        public IActionResult GetUser(Guid id)
        {
            VidconfileUser user = this.userServices.GetUserByIdWithVideos(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            GetUserApiModel model = this.mapper.Map<GetUserApiModel>(user);


            model.SubscriberCount = this.userServices.GetSubscriberCount(user);

            return Ok(model);
        }
    }
}