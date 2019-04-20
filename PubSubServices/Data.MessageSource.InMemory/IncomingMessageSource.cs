using Microsoft.Extensions.Logging;
using PubSubServices.Data.MessageSource.Interfaces;
using PubSubServices.Model.PubSub;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.Data.MessageSource.InMemory
{
    public class IncomingMessageSource : IIncomingMessageSource
    {
        #region Class Variables
        private readonly ILogger<IncomingMessageSource> _logger;
        #endregion

        #region Constructors
        public IncomingMessageSource(ILogger<IncomingMessageSource> logger)
        {
            _logger = logger;
        }
        #endregion

        #region IIncomingMessageSource Implementation
        public IList<IncomingPubSubMessageDescription> GetIncomingMessages()
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
