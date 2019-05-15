using System;
using System.IO;
using System.ServiceProcess;

using HostedService.Interfaces;

using MessagePublisherService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PubSubServices.Data.MessageSink.Interfaces;
using PubSubServices.Data.MessageSink.Log;
using PubSubServices.Data.MessageSource.InMemory;
using PubSubServices.Data.MessageSource.Interfaces;
using PubSubServices.ServiceHost.MessagePublisher;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace PubSubServices.ServiceHost.MessagePublisher
{
    class Program
    {
        #region Class Variables
        private static IServiceCollection _serviceCollection;
        private static IServiceProvider _serviceProvider;
        private static IConfiguration _configuration;
        #endregion

        #region Constants

        private const string EnvironmentIndicatingEnvironmentVariable = "ASPNETCORE_ENVIRONMENT";
        private const string LocalEnvironmentKey = "local";
        private const string ConfigFileName = "config";
        private const string ConfigFileExtension = "json";
        private const string AppComponentPropertyName = "AppComponent";
        #endregion

        static void Main(string[] args)
        {
            bool isConsole = Array.IndexOf(args, "--console") > -1 || Environment.CurrentDirectory != Environment.SystemDirectory;

            //basic service wireup
            _configuration = LoadConfiguration();
            _serviceCollection = InitializeServiceCollection();
            ConfigureOptions(_serviceCollection, _configuration);
            ConfigureLogger(_serviceCollection, _configuration);
            ConfigureDependencyInjection(_serviceCollection);
            _serviceProvider = _serviceCollection.BuildServiceProvider();

            var logger = _serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogInformation("Retrieved Logger from DI Container");

            using (ServiceBase service = _serviceProvider.GetService<ServiceBase>())
            {
                if (isConsole)
                {
                    //we won't be able to run as console unless the ServiceBase instance
                    //also implements IConsoleHostedService, so attempt the cast and signal failure
                    //IConsoleHostedService hostedService = service as IConsoleHostedService;
                    if (service is IConsoleHostedService hostedService)
                    {
                        hostedService.StartAsConsole(args);

                        logger.LogInformation($"Service {service.ServiceName} started successfully in console hosted mode");

                        logger.LogInformation($"Press any key to stop {service.ServiceName}.");
                        Console.ReadKey();

                        service.Stop();
                    }
                    else
                    {
                        throw new
                            ArgumentException("Cannot start provided ServiceBase implementation in Console hosted mode - it needs to also implement IConsoleHostedService");
                    }
                }
                else
                {
                    ServiceBase.Run(service);
                }
            }
        }

        #region Private Methods
        private static IConfiguration LoadConfiguration()
        {
            var environmentName = Environment.GetEnvironmentVariable(EnvironmentIndicatingEnvironmentVariable);


            if (environmentName == LocalEnvironmentKey)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"{ConfigFileName}.{environmentName}.{ConfigFileExtension}", optional: true);

                builder.AddEnvironmentVariables();

                _configuration = builder.Build();
            }
            else
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"{ConfigFileName}.{ConfigFileExtension}", optional: true);

                builder.AddEnvironmentVariables();

                _configuration = builder.Build();
            }

            return _configuration;

        }

        private static IServiceCollection InitializeServiceCollection()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddOptions();

            return serviceCollection;
        }

        private static void ConfigureOptions(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<LoggingOptions>(configuration.GetSection(nameof(LoggingOptions)));
        }

        private static void ConfigureLogger(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            string rollingFileLogPath = configuration.GetSection($"{nameof(LoggingOptions)}:{nameof(LoggingOptions.LogFilePath)}").Value;
            string appComponentName = configuration.GetSection($"{nameof(LoggingOptions)}:{nameof(LoggingOptions.AppComponentName)}").Value;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .Enrich.WithProperty(AppComponentPropertyName, appComponentName)
                .WriteTo.RollingFile(rollingFileLogPath).MinimumLevel.Debug()
                .WriteTo.Console(theme: SystemConsoleTheme.Literate).MinimumLevel.Information()
                //put seq info here
                .CreateLogger();

            serviceCollection.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
        }

        private static void ConfigureDependencyInjection(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IServiceCollection>(_serviceCollection);
            serviceCollection.AddSingleton<IConfiguration>(_configuration);
            serviceCollection.AddSingleton<ServiceBase, MessagePublisherScheduler>();
            //move to MessagePublisherScheduler
            //serviceCollection.AddSingleton<IOutgoingMessageSource, OutgoingMessageSource>();
            //serviceCollection.AddSingleton<IPubSubMessageSink, LogMessageSink>();
            //serviceCollection.AddSingleton<IMessagePublisherService, LogMessagePublisherService>();

            //_serviceProvider = serviceCollection.BuildServiceProvider();
        }
        #endregion


    }
}
