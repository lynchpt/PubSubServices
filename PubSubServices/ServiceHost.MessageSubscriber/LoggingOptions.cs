using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.ServiceHost.MessageSubscriber
{
    public class LoggingOptions
    {
        public string LogFilePath { get; set; }
        public string AppComponentName { get; set; }
        public string SeqApiKey { get; set; }
        public string SeqUrl { get; set; }
    }
}
