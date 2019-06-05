using PubSubServices.Model.PubSub;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PubSubServices.Service.MessageSubscriber
{
    public interface IMessageSubscriberService
    {
        Task<IList<IncomingPubSubMessageDescription>> ReceiveMessagesAsync(int receiveBatchSize);
    }
}
