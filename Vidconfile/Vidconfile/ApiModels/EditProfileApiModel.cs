using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vidconfile.ApiModels
{
    public class EditProfileApiModel
    {
        [Required]
        public string AuthorProfilePhotoUrl { get; set; }
    }
}
