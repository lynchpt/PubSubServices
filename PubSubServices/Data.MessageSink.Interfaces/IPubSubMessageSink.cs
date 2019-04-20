using PubSubServices.Model.PubSub;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.Data.MessageSink.Interfaces
{
    public interface IPubSubMessageSink
    {
        int PublishMessages(IList<OutgoingPubSubMessageDescription> messagesToPublish);
    }
}
