using PubSubServices.Model.PubSub;
using System.Collections.Generic;

namespace PubSubServices.Data.OutgoingMessage.Interfaces
{
    public interface IOutgoingMessageSource
    {
        IList<OutgoingPubSubMessageDescription> GetOutgoingMessages();
    }
}
