using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Linq;


using Microsoft.Extensions.Logging;
using PubSubServices.Model.PubSub;
using System.Threading.Tasks;
using PubSubServices.Data.Message.Interfaces;
using PubSubServices.Data.OutgoingMessage.Interfaces;

namespace PubSubServices.Service.MessagePublisher
{
    public class MessagePublisherService : IMessagePublisherService
    {
        #region Class Variables
        private readonly ILogger<MessagePublisherService> _logger;
        private readonly IOutgoingMessageSource _outgoingMessageSource;
        private readonly IOutgoingMessageSink _outgoingMessageSink;
        #endregion

        #region Constructors

        public MessagePublisherService(ILogger<MessagePublisherService> logger, IOutgoingMessageSource outgoingMessageSource,
            IOutgoingMessageSink outgoingMessageSink)
        {
            _logger = logger;
            _outgoingMessageSource = outgoingMessageSource;
            _outgoingMessageSink = outgoingMessageSink;
        }
        #endregion

        #region IMessagePublisherService Implementation
        public async Task<IList<PubSubMessagePublishResult>> PublishMessagesAsync()
        {
            //first we have to get from the source the messages we are going to publish to the sink
            IList<OutgoingPubSubMessageDescription> outgoingMessages = _outgoingMessageSource.GetOutgoingMessages();

            //now publish them to the sink
            IList<PubSubMessagePublishResult> publishResult = await _outgoingMessageSink.PublishMessagesAsync(outgoingMessages);

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
