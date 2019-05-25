using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.Model.PubSub
{
    public class ReadyToHandleMessageDescription
    {
        public int ParentOutgoingMessageDescriptionId { get; set; }
        public int SubscriptionId { get; set; }
        public string DestinationUrl { get; set; }


        public int ReadyToHandleMessageDescriptionId { get; set; }       
        public string OriginatorId { get; set; }        
        public string Body { get; set; }      
        public bool IsMessageHandled { get; set; }
        public int MessageHandlingAttempts { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDt { get; set; }
        public DateTime BeginDt { get; set; }
        public DateTime? EndDt { get; set; }
    }
}
