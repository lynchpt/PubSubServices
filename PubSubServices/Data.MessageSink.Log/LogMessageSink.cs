using Microsoft.Extensions.Logging;
using PubSubServices.Data.MessageSink.Interfaces;
using PubSubServices.Model.PubSub;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.Data.MessageSink.Log
{
    public class LogMessageSink : IPubSubMessageSink
    {
        #region Class Variables
        private readonly ILogger<LogMessageSink> _logger;
        #endregion

        #region Constructors
        public LogMessageSink(ILogger<LogMessageSink> logger)
        {
            _logger = logger;
        }
        #endregion

        #region IPubSubMessageSink Implementation

        public IList<PubSubMessagePublishResult> PublishMessages(IList<OutgoingPubSubMessageDescription> messagesToPublish)
        {
            IList<PubSubMessagePublishResult> publishResults = new List<PubSubMessagePublishResult>();

            if (messagesToPublish == null) return publishResults;

            foreach (var message in messagesToPublish)
            {
                string logEntry = $"Published Message to Log via {nameof(LogMessageSink)}: MessageId={message.MessageId}; body={message.Body}";

                _logger.LogInformation(logEntry);

                publishResults.Add(new PubSubMessagePublishResult() { MessageId = message.MessageId, WasSuccessfullyPublished = true });
            }

            return publishResults;
        }

        #endregion
    }
}
