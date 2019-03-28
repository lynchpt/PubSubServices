using System;

namespace FMGlobal.Lucid.Infra.Models.PubSub
{
    /// <summary>
    /// This class allows important information about incoming Pub Sub message fragments in a format that allows it
    /// to be stored in a tabular format for persistence.
    /// 
    /// It contains all the data needed to fully describe both the raw content and the metadata of an incoming PubSub
    /// message. Also included is metadata to allow tracking the status of this message as it is processed by application logic.
    /// 
    /// </summary>
    public class IncomingPubSubMessageDescriptionFragment: IIncomingPubSubMessageDescriptionFields
    {
        public int IncomingPubSubMessageDescriptionFragmentId { get; set; }
        public Guid MessageId { get; set; }
        public string OriginatorId { get; set; }
        public string MessageBodyVersion { get; set; }
        public string MessagePropertiesVersion { get; set; }
        public string BodyFullTypeName { get; set; }
        public string Body { get; set; }
        public string TopicName { get; set; }
        public string SubscriptionName { get; set; }
        public bool ProcessedByBusinessLogic { get; set; }
        public int BusinessLogicProcessingAttempts { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDt { get; set; }
        public DateTime BeginDt { get; set; }
        public DateTime? EndDt { get; set; }
    }
}
