using System;
using System.Collections.Generic;
using System.Text;

namespace Vidconfile.Data.Models
{
    public abstract class BaseDBModel
    {
        public Guid Id { get; set; }

        public DateTime Created { get; set; }
    }
}
