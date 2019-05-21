using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServicesData.MessageSink.ServiceBus
{
    public class KeyVaultConnectionInfoOptions
    {
        public string ConnectionStringStore { get; set; }
        public string AuthorizationStore { get; set; }
        public string ConnectionStringName { get; set; }
    }
}
