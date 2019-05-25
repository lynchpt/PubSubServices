using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.Model.PubSub
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }

        public string TopicNamespace { get; set; } //css environment
        public string TopicName { get; set; } //event type

        public string DestinationUrl { get; set; }

        //other metadata
    }
}
