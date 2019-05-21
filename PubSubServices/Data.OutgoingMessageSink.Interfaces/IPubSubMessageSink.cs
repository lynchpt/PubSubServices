using PubSubServices.Model.PubSub;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PubSubServices.Data.MessageSink.Interfaces
{
    public interface IPubSubMessageSink
    {
        Task<IList<PubSubMessagePublishResult>> PublishMessagesAsync(IList<OutgoingPubSubMessageDescription> messagesToPublish);
    }
}
