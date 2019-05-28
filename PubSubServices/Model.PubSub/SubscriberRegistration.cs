using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.Model.PubSub
{
    public class SubscriberRegistration
    {
        public int SubscriberId { get; set; }
        public int SubscriberName { get; set; }

        public string SubscriberDistributionList { get; set; }
    }
}
