using System;
using System.Collections.Generic;
using System.Text;

namespace Vidconfile.Data.Models
{
    public class VidconfileUserVideo
    {
        public Guid Id { get; set; }

        public Guid VidconfileUserId { get; set; }

        public VidconfileUser VidconfileUser { get; set; }

        public Guid VideoId { get; set; }

        public Video Video { get; set; }
    }
}
