using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Logging;

using PubSubServices.Data.OutgoingMessage.Interfaces;
using PubSubServices.Model.PubSub;

namespace PubSubServices.Data.OutgoingMessageSource.Json
{
    public class JsonOutgoingMessageSource : IOutgoingMessageSource
    {
        #region Class Variables
        private readonly ILogger<JsonOutgoingMessageSource> _logger;
        #endregion

        #region Constructors
        public JsonOutgoingMessageSource(ILogger<JsonOutgoingMessageSource> logger)
        {
            _logger = logger;
        }
        #endregion

        #region IOutgoingMessageSource Implementation
        public IList<OutgoingPubSubMessageDescription> GetOutgoingMessages()
        {
            IList<OutgoingPubSubMessageDescription> outgoingMessages = new List<OutgoingPubSubMessageDescription>();

            

            return outgoingMessages;
        }
        #endregion

        #region Private Methods
      
        #endregion
    }
}
