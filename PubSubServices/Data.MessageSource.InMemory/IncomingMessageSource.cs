using PubSubServices.Data.MessageSource.Interfaces;
using PubSubServices.Model.PubSub;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.Data.MessageSource.InMemory
{
    public class IncomingMessageSource : IIncomingMessageSource
    {
        #region Constructors
        public IncomingMessageSource()
        {

        }
        #endregion

        #region IIncomingMessageSource Implementation
        public IEnumerable<IncomingPubSubMessageDescription> GetIncomingMessages()
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
