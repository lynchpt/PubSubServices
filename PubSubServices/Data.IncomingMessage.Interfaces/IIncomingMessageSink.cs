using PubSubServices.Model.PubSub;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PubSubServices.Data.IncomingMessage.Interfaces
{
    public interface IIncomingMessageSink
    {
        Task<IList<PubSubMessageStoreResult>> StoreMessagesAsync(IList<IncomingPubSubMessageDescription> messagesToStore);
    }
}
