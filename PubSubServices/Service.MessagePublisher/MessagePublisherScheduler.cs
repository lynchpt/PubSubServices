using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PubSubServices.Data.MessageSink.Interfaces;
using PubSubServices.Data.MessageSink.Log;
using PubSubServices.Data.MessageSource.InMemory;
using PubSubServices.Data.MessageSource.Interfaces;
using PubSubServices.Model.PubSub;
using PubSubServicesData.MessageSink.ServiceBus;

namespace PubSubServices.Service.MessagePublisher
{
    public class MessagePublisherScheduler : ServiceBase, IMessagePublisherScheduler
    {
        #region Class Variables

        private readonly ILogger<MessagePublisherScheduler> _logger;
        private readonly IMessagePublisherService _messagePublisherService;
        //private readonly IConfiguration _configuration;
        private System.Timers.Timer _scheduler;


        #endregion

        #region Constructors

        public MessagePublisherScheduler(ILogger<MessagePublisherScheduler> logger, IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _logger = logger;

            ConfigureOptions(serviceCollection, configuration);
            ConfigureDependencyInjection(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            _messagePublisherService = serviceProvider.GetService<IMessagePublisherService>();


            ServiceName = _messagePublisherService.GetType().Name;
            //timer
            InitializeTimer();
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
            OnStart(args);
        }

        #endregion


        #region Private Methods

        private static void ConfigureOptions(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<ServiceBusEnvVarConnectionInfoOptions>(configuration.GetSection(nameof(ServiceBusEnvVarConnectionInfoOptions)));
        }

        private void ConfigureDependencyInjection(IServiceCollection serviceCollection)
        {
            //serviceCollection.AddSingleton<ServiceBase, MessagePublisherScheduler>();

            serviceCollection.AddScoped<IConnectionInfoProvider, EnvironmentVariableConnectionInfoProvider>();
            serviceCollection.AddScoped<ICredentialProvider, DefaultCredentialProvider>();
            serviceCollection.AddScoped<IOutgoingMessageSource, InMemoryOutgoingMessageSource>();
            //serviceCollection.AddSingleton<IPubSubMessageSink, LogMessageSink>();
            serviceCollection.AddSingleton<IPubSubMessageSink, ServiceBusMessageSink>();
            serviceCollection.AddSingleton<IMessagePublisherService, MessagePublisherService>();
        }


        private void InitializeTimer()
        {
            _scheduler = new System.Timers.Timer();
            _scheduler.Interval = 5000;
            _scheduler.AutoReset = false;
            _scheduler.Elapsed += OnElapsed;
            _scheduler.Enabled = true;
        }

        private async void OnElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _logger.LogInformation(
                    $"{nameof(MessagePublisherScheduler)} {nameof(OnElapsed)} method called");

                IList<PubSubMessagePublishResult> publishResults = await _messagePublisherService.PublishMessagesAsync();

                //if we succeeded (no exception generated), we know we can enable the timer
                //to fire one more event
                _scheduler.Enabled = true;
            }
            catch (Exception ex)
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
    }
}
