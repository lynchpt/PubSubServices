using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HostedService.Interfaces;
using PubSubServices.Model.PubSub;

namespace PubSubServices.Service.MessagePublisher
{
    public interface IMessagePublisherService
    {
        Task<IList<PubSubMessagePublishResult>> PublishMessagesAsync();
    }
}
