using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Text;

using Microsoft.Extensions.Logging;

namespace MessagePublisherService
{
    public class LogMessagePublisherService : IMessagePublisherService
    {
        #region Class Variables
        private readonly ILogger<LogMessagePublisherService> _logger;
        #endregion

        #region Constructors

        public LogMessagePublisherService(ILogger<LogMessagePublisherService> logger)
        {
            _logger = logger;
        }
        #endregion

        #region IMessagePublisherService Implementation
        public void PublishMessages()
        {
            //_scheduler is already disabled at this point; no more events will come through until we reset it
            //by _scheduler.Enabled = true;
            try
            {
                if (DateTime.UtcNow.Ticks % 11 == 0)
                {
                    throw new DivideByZeroException("Divide By Zero");
                }
                if (DateTime.UtcNow.Ticks % 3 == 0)
                {
                    throw new Exception("Handled Error");
                }

                _logger.LogInformation("Published message");

            }
            catch (DivideByZeroException dbze)
            {
                _logger.LogCritical($"Fatal Error: {dbze.Message}");
                //example of error where we want to stop the process from trying to run again.
                //this is done by throwing the exception, which will result in the scheduler
                //not resetting the timer event

                throw;
            }
            catch (Exception ex)
            {
                //example of error where we want the process try again.
                //this is done by handling the error so the containing scheduler sees no error
                //and resetting the timer event

                //handle
                _logger.LogError($"Handled Error: {ex.Message}");
            }
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
