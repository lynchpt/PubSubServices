using PubSubServices.Model.PubSub;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.Data.IncomingMessage.Interfaces
{
    public interface IIncomingMessageSource
    {
       IList<IncomingPubSubMessageDescription> ReceiveIncomingMessages(int receiveMessageBatchSize);
    }
}
