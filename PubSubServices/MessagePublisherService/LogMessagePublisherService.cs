using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Linq;
using PubSubServices.Data.MessageSource.Interfaces;
using PubSubServices.Data.MessageSink.Interfaces;


using Microsoft.Extensions.Logging;
using PubSubServices.Model.PubSub;
using System.Threading.Tasks;

namespace MessagePublisherService
{
    public class LogMessagePublisherService : IMessagePublisherService
    {
        #region Class Variables
        private readonly ILogger<LogMessagePublisherService> _logger;
        private readonly IOutgoingMessageSource _outgoingMessageSource;
        private readonly IPubSubMessageSink _pubSubMessageSink;
        #endregion

        #region Constructors

        public LogMessagePublisherService(ILogger<LogMessagePublisherService> logger, IOutgoingMessageSource outgoingMessageSource, 
            IPubSubMessageSink pubSubMessageSink)
        {
            _logger = logger;
            _outgoingMessageSource = outgoingMessageSource;
            _pubSubMessageSink = pubSubMessageSink;
        }
        #endregion

        #region IMessagePublisherService Implementation
        public async Task<IList<PubSubMessagePublishResult>> PublishMessagesAsync()
        {
            //first we have to get from the source the messages we are going to publish to the sink
            IList<OutgoingPubSubMessageDescription> outgoingMessages = _outgoingMessageSource.GetOutgoingMessages();

            //now publish them to the sink
            IList<PubSubMessagePublishResult> publishResult = await _pubSubMessageSink.PublishMessagesAsync(outgoingMessages);

            //TODO: inform the MessageSource of the publish status (successful or not) of each message;

            int successfullyPublished = publishResult.Where(r => r.WasSuccessfullyPublished == true).Select(r => r).ToList().Count;
            _logger.LogInformation($"attempted to publish {outgoingMessages?.Count}; successfully published {successfullyPublished}");

            return publishResult;
        }

        //public void PublishMessages()
        //{
        //    //_scheduler is already disabled at this point; no more events will come through until we reset it
        //    //by _scheduler.Enabled = true;
        //    try
        //    {
        //        if (DateTime.UtcNow.Ticks % 11 == 0)
        //        {
        //            throw new DivideByZeroException("Divide By Zero");
        //        }
        //        if (DateTime.UtcNow.Ticks % 3 == 0)
        //        {
        //            throw new Exception("Handled Error");
        //        }

        //        _logger.LogInformation("Published message");

        //    }
        //    catch (DivideByZeroException dbze)
        //    {
        //        _logger.LogCritical($"Fatal Error: {dbze.Message}");
        //        //example of error where we want to stop the process from trying to run again.
        //        //this is done by throwing the exception, which will result in the scheduler
        //        //not resetting the timer event

        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        //example of error where we want the process try again.
        //        //this is done by handling the error so the containing scheduler sees no error
        //        //and resetting the timer event

        //        //handle
        //        _logger.LogError($"Handled Error: {ex.Message}");
        //    }
        //}
        #endregion

        #region Private Methods

        #endregion
    }
}
