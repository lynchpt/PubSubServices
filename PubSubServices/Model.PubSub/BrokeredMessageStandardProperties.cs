using System;
using System.Collections.Generic;

namespace PubSubServices.Model.PubSub
{
    /// <summary>
    /// Contains metadata information that is common to all Pub Sub messages
    /// </summary>
    public class BrokeredMessageStandardProperties
    {
        public string MessagePropertiesVersion { get; set; }
        //this relies on the first character of MessageBodyVersion being a number, which will be the case in semantic versioning
        public int MessagePropertiesMajorVersionNumber => Convert.ToInt32(MessagePropertiesVersion.Substring(0, 1));
        public string MessageBodyVersion { get; set; }
        //this relies on the first character of MessageBodyVersion being a number, which will be the case in semantic versioning
        public int MessageBodyMajorVersionNumber => Convert.ToInt32(MessageBodyVersion.Substring(0, 1));
        public string BodyFullTypeName { get; set; }
        public string OriginatorId { get; set; }

        /// <summary>
        /// Convenience method to harvest the class's information into a dictionary for use
        /// with some Pub Sub message SDKs.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> GetStandardPropertiesAsDictionary()
        {
            var dictionary = new Dictionary<string, object>()
            {
                {nameof(MessagePropertiesVersion), MessagePropertiesVersion},
                {nameof(MessagePropertiesMajorVersionNumber), MessagePropertiesMajorVersionNumber},
                {nameof(MessageBodyVersion), MessageBodyVersion},
                {nameof(MessageBodyMajorVersionNumber), MessageBodyMajorVersionNumber},
                {nameof(BodyFullTypeName), BodyFullTypeName},
                {nameof(OriginatorId), OriginatorId}
            };

            return dictionary;
        }
    }
}
