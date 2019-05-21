using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Linq;
using PubSubServices.Data.MessageSource.Interfaces;
using PubSubServices.Data.MessageSink.Interfaces;


using Microsoft.Extensions.Logging;
using PubSubServices.Model.PubSub;
using System.Threading.Tasks;

namespace PubSubServices.Service.MessagePublisher
{
    public class MessagePublisherService : IMessagePublisherService
    {
        #region Class Variables
        private readonly ILogger<MessagePublisherService> _logger;
        private readonly IOutgoingMessageSource _outgoingMessageSource;
        private readonly IPubSubMessageSink _pubSubMessageSink;
        #endregion

        #region Constructors

        public MessagePublisherService(ILogger<MessagePublisherService> logger, IOutgoingMessageSource outgoingMessageSource,
            IPubSubMessageSink pubSubMessageSink)
        {
            _logger = logger;
            _outgoingMessageSource = outgoingMessageSource;
            _pubSubMessageSink = pubSubMessageSink;
        }
        #endregion

        #region IMessagePublisherService Implementation
        public async Task<IList<PubSubMessagePublishResult>> PublishMessagesAsync()
        {
            //first we have to get from the source the messages we are going to publish to the sink
            IList<OutgoingPubSubMessageDescription> outgoingMessages = _outgoingMessageSource.GetOutgoingMessages();

            //now publish them to the sink
            IList<PubSubMessagePublishResult> publishResult = await _pubSubMessageSink.PublishMessagesAsync(outgoingMessages);

            //TODO: inform the MessageSource of the publish status (successful or not) of each message;

            int successfullyPublished = publishResult.Where(r => r.WasSuccessfullyPublished == true).Select(r => r).ToList().Count;
            _logger.LogInformation($"attempted to publish {outgoingMessages?.Count}; successfully published {successfullyPublished}");

            return publishResult;
        }   
        #endregion

        #region Private Methods

        #endregion
    }
}
