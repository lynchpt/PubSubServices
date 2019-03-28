using System;

namespace FMGlobal.Lucid.Infra.Models.PubSub
{
    public interface IOutgoingPubSubMessageDescription
    {
        #region Properties, Indexers

        DateTime BeginDt { get; set; }

        string Body { get; set; }

        string BodyFullTypeName { get; set; }

        string CreateBy { get; set; }

        DateTime CreateDt { get; set; }

        DateTime? EndDt { get; set; }

        string MessageBodyVersion { get; set; }

        Guid MessageId { get; set; }

        string MessagePropertiesVersion { get; set; }

        string OriginatorId { get; set; }

        int OutgoingPubSubMessageDescriptionId { get; set; }

        int PubSubSubmissionAttempts { get; set; }

        string TopicName { get; set; }

        string TopicNamespace { get; set; }

        string UpdateBy { get; set; }

        DateTime UpdateDt { get; set; }

        bool WasSentToPubSub { get; set; }

        #endregion
    }
}