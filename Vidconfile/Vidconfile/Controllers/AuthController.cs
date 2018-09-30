using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Vidconfile.ApiModels;
using Vidconfile.Constants;
using Vidconfile.Data.Models;
using Vidconfile.Services.Services;

namespace Vidconfile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IVidconfileUserServices userServices;
        private readonly IConfiguration config;

        public AuthController(IVidconfileUserServices userServices, IConfiguration config)
        {
            this.userServices = userServices ?? throw new NullReferenceException("userServices cannot be null or empty");
            this.config = config ?? throw new NullReferenceException("config cannot be null or empty");
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterUserApiModel registerUser)
        {
            if (this.userServices.UserExists(registerUser.Username))
            {
                return BadRequest("Username already exists");
            }

            this.userServices.Register(registerUser.Username, registerUser.Password, registerUser.ProfilePhotoUrl);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUserApiModel loginUser)
        {
            VidconfileUser user = this.userServices.Login(loginUser.Username, loginUser.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config.GetSection(AppSettingsConstants.Token).Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
        
        [HttpGet("verifytoken")]
        public IActionResult VerifyToken()
        {
            if (this.User == null || this.User.Claims == null || this.User.Claims.Count() == 0 ||
                string.IsNullOrEmpty(this.User.Claims.FirstOrDefault(x => !x.Properties.FirstOrDefault(v => v.Value == "nameid").Equals(default(KeyValuePair<string, string>))).Value))
            {
                return Ok(false);
            }

            Guid cl = Guid.Parse(this.User.Claims
               .FirstOrDefault(x => !x.Properties.FirstOrDefault(v => v.Value == "nameid").Equals(default(KeyValuePair<string, string>)))
               .Value);

            bool userExists = this.userServices.UserExists(cl);

            return Ok(userExists);
        }
    }
}