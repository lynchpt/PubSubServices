using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.Model.PubSub
{
    public class PubSubMessageStoreResult
    {
        public IncomingPubSubMessageDescription IncomingPubSubMessageDescription { get; set; }
        public bool WasSuccessfullyStored { get; set; }
    }
}
