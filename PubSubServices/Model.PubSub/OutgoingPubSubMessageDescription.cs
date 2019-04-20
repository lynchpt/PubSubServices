using System;

namespace PubSubServices.Model.PubSub
{
    /// <summary>
    /// This class allows important information about outgoing Pub Sub messages in a format that allows it
    /// to be stored in a tabular format for persistence.
    /// 
    /// It contains all the data needed to fully describe both the raw content and the metadata of an outgoing PubSub
    /// message. Also included is metadata to allow tracking the status of this message as it is delivered to a Pub Sub repository.
    /// 
    /// NOTE: DO NOT CHANGE THE ORDER OF THE PROPERTIES OF THIS CLASS.
    /// NOTE: THE CODE THAT CONSUMES THIS TYPE IN ADOHELPER IS SENSITIVE TO THE PROPERTY ORDER.
    /// </summary>
    public class OutgoingPubSubMessageDescription
    {
        #region Properties, Indexers

        public int OutgoingPubSubMessageDescriptionId { get; set; }

        public Guid MessageId { get; set; }

        public string OriginatorId { get; set; }

        public string MessagePropertiesVersion { get; set; }

        public string MessageBodyVersion { get; set; }

        public string BodyFullTypeName { get; set; }

        public string Body { get; set; }

        public string TopicNamespace { get; set; }

        public string TopicName { get; set; }

        public bool WasSentToPubSub { get; set; }

        public int PubSubSubmissionAttempts { get; set; }

        public string CreateBy { get; set; }

        public DateTime CreateDt { get; set; }

        public string UpdateBy { get; set; }

        public DateTime UpdateDt { get; set; }

        public DateTime BeginDt { get; set; }

        public DateTime? EndDt { get; set; }

        #endregion
    }
}