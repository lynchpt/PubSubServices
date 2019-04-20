using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.Model.PubSub
{
    public class PubSubMessagePublishResult
    {
        public Guid MessageId { get; set; }
        public bool WasSuccessfullyPublished { get; set; }
    }
}
