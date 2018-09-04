using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vidconfile.Constants;

namespace Vidconfile.ApiModels
{
    public class LoginUserApiModel
    {
        [Required]
        [StringLength(UserConstants.MaxUsernameLength, MinimumLength = UserConstants.MinUsernameLength, ErrorMessage = "Your username length is invalid")]
        public string Username { get; set; }

        [Required]
        [StringLength(UserConstants.MaxPasswordLength, MinimumLength = UserConstants.MinPasswordLength, ErrorMessage = "Your password length is invalid")]
        public string Password { get; set; }
    }
}
