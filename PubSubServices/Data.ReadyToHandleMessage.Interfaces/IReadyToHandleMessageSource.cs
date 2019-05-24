using System;
using System.Collections.Generic;
using System.Text;

using PubSubServices.Model.PubSub;

namespace PubSubServices.Data.ReadyToHandleMessage.Interfaces
{
    public interface IReadyToHandleMessageSource
    {
        IList<ReadyToHandleMessageDescription> GetReadyToHandleMessages();
    }
}
