using PubSubServices.Data.IncomingMessage.Interfaces;
using PubSubServices.Model.PubSub;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.Data.IncomingMessageSource.InMemory
{
    public class InMemoryIncomingMessageSource : IIncomingMessageSource
    {
        public IList<IncomingPubSubMessageDescription> ReceiveIncomingMessages(int receiveMessageBatchSize)
        {
            throw new NotImplementedException();
        }
    }
}
