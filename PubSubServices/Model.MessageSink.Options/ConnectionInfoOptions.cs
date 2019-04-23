using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.Model.MessageSink.Options
{
    public class ConnectionInfoOptions
    {
        public string ConnectionStringStore { get; set; }
        public string AuthorizationStore { get; set; }
        public string ConnectionStringName { get; set; }
    }
}
