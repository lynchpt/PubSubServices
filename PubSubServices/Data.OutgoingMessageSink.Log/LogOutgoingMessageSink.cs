using Microsoft.Extensions.Logging;
using PubSubServices.Data.Message.Interfaces;
using PubSubServices.Data.OutgoingMessage.Interfaces;
using PubSubServices.Model.PubSub;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;



namespace PubSubServices.Data.MessageSink.Log
{
    public class LogOutgoingMessageSink : IOutgoingMessageSink
    {
        #region Class Variables
        private readonly ILogger<LogOutgoingMessageSink> _logger;
        #endregion

        #region Constructors
        public LogOutgoingMessageSink(ILogger<LogOutgoingMessageSink> logger)
        {
            _logger = logger;
        }
        #endregion

        #region IOutgoingMessageSink Implementation

        public Task<IList<PubSubMessagePublishResult>> PublishMessagesAsync(IList<OutgoingPubSubMessageDescription> messagesToPublish)
        {
            IList<PubSubMessagePublishResult> publishResults = new List<PubSubMessagePublishResult>();

            if (messagesToPublish == null || messagesToPublish.Count == 0) return Task.FromResult(publishResults);

            foreach (var message in messagesToPublish)
            {
                string logEntry = $"Published Message to Log via {nameof(LogOutgoingMessageSink)}: MessageId={message.MessageId}; body={message.Body}";

                _logger.LogInformation(logEntry);

                publishResults.Add(new PubSubMessagePublishResult() { MessageId = message.MessageId, WasSuccessfullyPublished = true });
            }

            return Task.FromResult(publishResults);
        }

        #endregion
    }
}
