using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Text;

using Microsoft.Extensions.Logging;

namespace MessagePublisherService
{
    public class MessagePublisherScheduler : ServiceBase, IMessagePublisherScheduler
    {
        #region Class Variables

        private readonly ILogger<MessagePublisherScheduler> _logger;
        private readonly IMessagePublisherService _messagePublisherService;

        private System.Timers.Timer _scheduler;


        #endregion

        #region Constructors
        public MessagePublisherScheduler(ILogger<MessagePublisherScheduler> logger,
            IMessagePublisherService messagePublisherService)
        {
            _logger = logger;
            _messagePublisherService = messagePublisherService;

            ServiceName = _messagePublisherService.GetType().Name;
            //timer
            InitializeTimer();
        }

        private void OnElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _logger.LogInformation(
                    $"{nameof(MessagePublisherScheduler)} {nameof(OnElapsed)} method called" );

                _messagePublisherService.PublishMessages();

                //if we succeeded (no exception generated), we know we can enable the timer
                //to fire one more event
                _scheduler.Enabled = true;
            }
            catch ( Exception ex )
            {
                //handle (usually logging)
                //since we are not re-enabling the timer, no more events will get fired.
                _logger.LogError($"Error in {nameof(OnElapsed)} method: {ex.Message}");
            }
            finally
            {
                _logger.LogInformation($"_scheduler.Enabled set to {_scheduler.Enabled}");
            }
        }
        #endregion

        #region ServiceBase Overrides
        protected override void OnStart(string[] args)
        {
            _logger.LogInformation($"{nameof(OnStart)} method called");
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            _logger.LogInformation($"{nameof(OnStop)} method called");
        }
        #endregion

        #region IConsoleHostedService Implementation
        public void StartAsConsole(string[] args)
        {
            _logger.LogInformation($"{nameof(OnStop)} method called");
            this.OnStart(args);
        }

        #endregion


        #region Private Methods

        private void InitializeTimer()
        {
            _scheduler = new System.Timers.Timer();
            _scheduler.Interval = 5000;
            _scheduler.AutoReset = false;
            _scheduler.Elapsed += OnElapsed;
            _scheduler.Enabled = true;
        }
        #endregion
    }
}
