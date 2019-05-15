using Microsoft.Extensions.Logging;
using PubSubServices.Data.MessageSource.Interfaces;
using PubSubServices.Model.PubSub;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.Data.MessageSource.InMemory
{
    public class InMemoryOutgoingMessageSource : IOutgoingMessageSource
    {
        #region Class Variables
        private readonly ILogger<InMemoryOutgoingMessageSource> _logger;
        #endregion

        #region Constructors
        public InMemoryOutgoingMessageSource(ILogger<InMemoryOutgoingMessageSource> logger)
        {
            _logger = logger;
        }
        #endregion

        #region IOutgoingMessageSource Implementation
        public IList<OutgoingPubSubMessageDescription> GetOutgoingMessages()
        {
            IList<OutgoingPubSubMessageDescription> outgoingMessages = new List<OutgoingPubSubMessageDescription>();

            outgoingMessages = GenerateOutgoingMessages();

            return outgoingMessages;
        }
        #endregion

        #region Private Methods
        private IList<OutgoingPubSubMessageDescription> GenerateOutgoingMessages()
        {
            IList<OutgoingPubSubMessageDescription> outgoingMessages = new List<OutgoingPubSubMessageDescription>()
            {
                new OutgoingPubSubMessageDescription()
                {
                    OutgoingPubSubMessageDescriptionId = 1,
                    MessageId = Guid.NewGuid(),
                    OriginatorId = "PubSubServices",

                    Body = GenerateMessageBody(),
                    BodyFullTypeName = "Model.Message.SampleTopic.SampleTopicMessageBody",

                    TopicName = "InsuranceBinderPremiumEstablished",
                    TopicNamespace = "DEV01",

                    PubSubSubmissionAttempts = 0,
                    WasSentToPubSub = false,

                    MessageBodyVersion = "0.1.0",
                    MessagePropertiesVersion = "0.1.0",


                    BeginDt = DateTime.UtcNow,
                    EndDt = null,
                    CreateDt = DateTime.UtcNow,
                    CreateBy = nameof(InMemoryOutgoingMessageSource),
                    UpdateDt = DateTime.UtcNow,
                    UpdateBy = nameof(InMemoryOutgoingMessageSource),
                }
            };


            return outgoingMessages;
        }

        private string GenerateMessageBody()
        {
            Random rand = new Random();

            int id = rand.Next(1, 10);
            string name = $"Name for {id}";
            string description = $"Description for {id}";

            string messageBody = $"{{\"id\": {id}, \"name\": {name}, \"description\": {description}}}";

            return messageBody;

        }
        #endregion
    }
}
