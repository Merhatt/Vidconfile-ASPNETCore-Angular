using System;
using System.Collections.Generic;
using System.Text;

namespace Vidconfile.Data.Models
{
    public class SubscribeToSubscribers
    {
        public Guid Id { get; set; }

        public Guid SubscriberId { get; set; }

        public Guid SubscribedToId { get; set; }
    }
}
