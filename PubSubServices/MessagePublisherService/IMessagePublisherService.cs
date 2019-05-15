using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HostedService.Interfaces;
using PubSubServices.Model.PubSub;

namespace MessagePublisherService
{
    public interface IMessagePublisherService
    {
        Task<IList<PubSubMessagePublishResult>> PublishMessagesAsync();
    }
}
