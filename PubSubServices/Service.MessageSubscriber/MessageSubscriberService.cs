using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PubSubServices.Data.IncomingMessage.Interfaces;
using PubSubServices.Data.IncomingMessageSink.Log;
using PubSubServices.Data.IncomingMessageSource.InMemory;
using PubSubServices.Data.Message.Interfaces;
using PubSubServices.Infra.ServiceBus;
using PubSubServices.Model.PubSub;

namespace PubSubServices.Service.MessageSubscriber
{
    public class MessageSubscriberService : ServiceBase, IMessageSubscriberService
    {
        #region Class Variables

        private readonly ILogger<MessageSubscriberService> _logger;
        private readonly IConfiguration _configuration;
        private System.Timers.Timer _scheduler;
        #endregion


        #region Constructors
        public MessageSubscriberService(ILogger<MessageSubscriberService> logger, IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _logger = logger;

            ConfigureOptions(serviceCollection, configuration);
            ConfigureDependencyInjection(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            
            ServiceName = nameof(MessageSubscriberService);
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
            _logger.LogInformation($"{nameof(OnStart)} method called");
            OnStart(args);
        }

        #endregion

        #region IMessageSubscriberService Implementation
        public Task<IList<IncomingPubSubMessageDescription>> ReceiveMessagesAsync(int receiveBatchSize)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Configuration Private Methods
        private static void ConfigureOptions(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            //serviceCollection.Configure<ServiceBusEnvVarConnectionInfoOptions>(configuration.GetSection(nameof(ServiceBusEnvVarConnectionInfoOptions)));
            serviceCollection.Configure<KeyVaultConnectionInfoOptions>(configuration.GetSection(nameof(KeyVaultConnectionInfoOptions)));
        }

        private void ConfigureDependencyInjection(IServiceCollection serviceCollection)
        {
            //serviceCollection.AddSingleton<ServiceBase, MessagePublisherScheduler>();

            serviceCollection.AddScoped<IConnectionInfoProvider, EnvironmentVariableConnectionInfoProvider>();
            //serviceCollection.AddScoped<IConnectionInfoProvider, AzureKeyVaultConnectionInfoProvider>();
            //serviceCollection.AddScoped<ICredentialProvider, DefaultCredentialProvider>();
            serviceCollection.AddScoped<IIncomingMessageSource, InMemoryIncomingMessageSource>();
            //serviceCollection.AddSingleton<IOutgoingMessageSink, ServiceBusOutgoingMessageSink>();
            serviceCollection.AddSingleton<IIncomingMessageSink, LogIncomingMessageSink>();
        }

        #endregion

        #region Scheduling Private Methods
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
                _logger.LogInformation($"{nameof(MessageSubscriberService)} {nameof(OnElapsed)} method called");

                //IList<PubSubMessagePublishResult> publishResults = await _messagePublisherService.PublishMessagesAsync();

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
