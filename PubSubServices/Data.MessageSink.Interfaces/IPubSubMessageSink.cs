using PubSubServices.Model.PubSub;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.Data.MessageSink.Interfaces
{
    public interface IPubSubMessageSink
    {
        IList<PubSubMessagePublishResult> PublishMessages(IList<OutgoingPubSubMessageDescription> messagesToPublish);
    }
}
