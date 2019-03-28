using System;

namespace FMGlobal.Lucid.Infra.Models.PubSub
{
    public interface IIncomingPubSubMessageDescriptionFields
    {
        Guid MessageId { get; set; }
        string OriginatorId { get; set; }
        string MessageBodyVersion { get; set; }
        string MessagePropertiesVersion { get; set; }
        string BodyFullTypeName { get; set; }
        string Body { get; set; }
        string TopicName { get; set; }
        string SubscriptionName { get; set; }
        bool ProcessedByBusinessLogic { get; set; }
        int BusinessLogicProcessingAttempts { get; set; }
        string CreateBy { get; set; }
        DateTime CreateDt { get; set; }
        string UpdateBy { get; set; }
        DateTime UpdateDt { get; set; }
        DateTime BeginDt { get; set; }
        DateTime? EndDt { get; set; }
    }
}
