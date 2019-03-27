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

        private ILogger<MessagePublisherScheduler> _logger;

        private System.Timers.Timer _scheduler;

        private readonly IMessagePublisherService _messagePublisherService;
        #endregion

        #region Constructors
        public MessagePublisherScheduler(ILogger<MessagePublisherScheduler> logger)
        {
            _logger = logger;

            _logger.LogInformation("MessagePublisherScheduler constructed");

            _messagePublisherService = new LogMessagePublisherService();
            ServiceName = _messagePublisherService.GetType().Name;
            //timer
            InitializeTimer();
        }

        private void OnElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _messagePublisherService.PublishMessages();

                //if we succeeded (no exception generated), we know we can enable the timer
                //to fire one more event
                _scheduler.Enabled = true;
            }
            catch (Exception ex)
            {
                //handle (usually logging)
                //since we are not re-enabling the timer, no more events will get fired.
            }
            
        }
        #endregion

        #region ServiceBase Overrides
        protected override void OnStart(string[] args)
        {
            //string filename = LogMessagePublisherService.CheckFileExists();
            //File.AppendAllText(filename, $"{DateTime.Now} started.{Environment.NewLine}");
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            //string filename = LogMessagePublisherService.CheckFileExists();
            //File.AppendAllText(filename, $"{DateTime.Now} stopped.{Environment.NewLine}");
        }
        #endregion

        #region IConsoleHostedService Implementation
        public void StartAsConsole(string[] args)
        {
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
