using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vidconfile.ApiModels;
using Vidconfile.Data.Models;
using Vidconfile.Services.Services;

namespace Vidconfile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IVidconfileUserServices userServices;

        public AuthController(IVidconfileUserServices userServices)
        {
            this.userServices = userServices ?? throw new NullReferenceException("userServices cannot be null or empty");
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterUserApiModel registerUser)
        {
            //validate request

            if (this.userServices.UserExists(registerUser.Username))
            {
                return BadRequest("Username already exists");
            }

            VidconfileUser userToCreate = new VidconfileUser();

            userToCreate.Username = registerUser.Username;

            VidconfileUser createdUser = this.userServices.Register(userToCreate, registerUser.Password);

            return StatusCode(201);
        }
    }
}