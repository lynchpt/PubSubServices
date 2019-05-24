using PubSubServices.Model.PubSub;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PubSubServices.Data.OutgoingMessage.Interfaces
{
    public interface IOutgoingMessageSink
    {
        Task<IList<PubSubMessagePublishResult>> PublishMessagesAsync(IList<OutgoingPubSubMessageDescription> messagesToPublish);
    }
}
