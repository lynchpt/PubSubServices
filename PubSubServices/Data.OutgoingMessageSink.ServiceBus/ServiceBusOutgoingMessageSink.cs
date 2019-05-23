using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Logging;
using PubSubServices.Data.Message.Interfaces;
using PubSubServices.Model.PubSub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubServicesData.MessageSink.ServiceBus
{
    public class ServiceBusOutgoingMessageSink : IOutgoingMessageSink
    {
        #region Class Variables
        private readonly ILogger<ServiceBusOutgoingMessageSink> _logger;
        private readonly IConnectionInfoProvider _connectionInfoProvider;
        private string _connectionString = null;
        private IDictionary<string, IMessageSender> _messageSenders = new Dictionary<string, IMessageSender>();
        #endregion

        #region Constants
        private const string MessageContentType = "application/json;charset=utf-16";
        #endregion

        #region Constructors
        public ServiceBusOutgoingMessageSink(ILogger<ServiceBusOutgoingMessageSink> logger, IConnectionInfoProvider connectionInfoProvider)
        {
            _logger = logger;
            _connectionInfoProvider = connectionInfoProvider;
        }
        #endregion

        #region IOutgoingMessageSink Implementation
        public async Task<IList<PubSubMessagePublishResult>> PublishMessagesAsync(IList<OutgoingPubSubMessageDescription> messagesToPublish)
        {
            IList<PubSubMessagePublishResult> messagePublishResults = new List<PubSubMessagePublishResult>();

            if(messagesToPublish == null || messagesToPublish.Count == 0)
            {
                return messagePublishResults;
            }

            //get list of topics that the messages target
            IList<string> topics = messagesToPublish.Select(m => m.TopicName).ToList();
            await EnsureMessageSenders(_connectionInfoProvider, topics);


            foreach (var outgoingMessage in messagesToPublish)
            {
                try
                {
                    //set metadata properties
                    BrokeredMessageStandardProperties standardProperties = new BrokeredMessageStandardProperties()
                    {
                        BodyFullTypeName = outgoingMessage.BodyFullTypeName,
                        MessageBodyVersion = outgoingMessage.MessageBodyVersion,
                        MessagePropertiesVersion = outgoingMessage.MessagePropertiesVersion,
                        OriginatorId = outgoingMessage.OriginatorId
                    };
                    
                    //fill in other message properties
                    Message message = new Message();
                    message.ContentType = MessageContentType;
                    byte[] bodyAsBytes = Encoding.Unicode.GetBytes(outgoingMessage.Body);
                    message.MessageId = outgoingMessage.MessageId.ToString();
                    message.Body = bodyAsBytes;

                    //add metadata properties
                    var propertiesAsDictionary = standardProperties.GetStandardPropertiesAsDictionary();
                    foreach (var entry in propertiesAsDictionary)
                    {
                        message.UserProperties.Add(entry);
                    }

                    //send message
                    IMessageSender sender;
                    _messageSenders.TryGetValue(outgoingMessage.TopicName, out sender);

                    if(sender != null)
                    {
                        await sender.SendAsync(message);
                    }
                    else
                    {
                        _logger.LogWarning($"Failure sending message with id {outgoingMessage.MessageId.ToString()} to topic {outgoingMessage.TopicName}; no MessageSender found.");
                        messagePublishResults.Add(new PubSubMessagePublishResult() { MessageId = outgoingMessage.MessageId, WasSuccessfullyPublished = false });
                    }
                    
                    _logger.LogInformation($"Success sending message with id {outgoingMessage.MessageId.ToString()} to topic {outgoingMessage.TopicName}");
                    messagePublishResults.Add(new PubSubMessagePublishResult() { MessageId = outgoingMessage.MessageId, WasSuccessfullyPublished = true });
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Failure sending message with id {outgoingMessage.MessageId.ToString()} to topic {outgoingMessage.TopicName}");
                    messagePublishResults.Add(new PubSubMessagePublishResult() { MessageId = outgoingMessage.MessageId, WasSuccessfullyPublished = false });                    
                }

            }
            return messagePublishResults;
        }
        #endregion

        #region Private Methods

        private async Task EnsureMessageSenders(IConnectionInfoProvider connectionInfoProvider, IList<string> messageDestinations)
        {
            if(_connectionString == null)
            {
                _connectionString = await connectionInfoProvider.GetConnectionStringAsync();
            }

            foreach(string target in messageDestinations)
            {
                if (!_messageSenders.ContainsKey(target))
                {
                    IMessageSender sender = new MessageSender(_connectionString, target);
                    _messageSenders.Add(target, sender);
                }
            }
            
        }
        #endregion
    }
}
