using System;
using System.ServiceProcess;

using HostedService.Interfaces;

using MessagePublisherService;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PubSubServices.Data.MessageSink.Interfaces;
using PubSubServices.Data.MessageSink.Log;
using PubSubServices.Data.MessageSource.InMemory;
using PubSubServices.Data.MessageSource.Interfaces;

namespace PubSubServices.PublisherService
{
    class Program
    {
        #region Class Variables
        private static IServiceProvider _serviceProvider;
        #endregion

        static void Main(string[] args)
        {
            bool isConsole = Array.IndexOf(args, "--console") > -1 || Environment.CurrentDirectory != Environment.SystemDirectory;

            _serviceProvider = ConfigureDependencyInjection();

            var logger = _serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            logger.LogInformation("Retrieved Logger from DI Container");

            using (ServiceBase service = _serviceProvider.GetService<ServiceBase>())
            {
                if ( isConsole )
                {
                    //we won't be able to run as console unless the ServiceBase instance
                    //also implements IConsoleHostedService, so attempt the cast and signal failure
                    //IConsoleHostedService hostedService = service as IConsoleHostedService;
                    if (service is IConsoleHostedService hostedService )
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
                    ServiceBase.Run((ServiceBase)service);
                }
            }
        }

        #region Private Methods
        private static IServiceProvider ConfigureDependencyInjection()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging( lb => lb.AddConsole() );
            serviceCollection.AddSingleton<ServiceBase, MessagePublisherScheduler>();
            serviceCollection.AddSingleton<IOutgoingMessageSource, OutgoingMessageSource>();
            serviceCollection.AddSingleton<IPubSubMessageSink, LogMessageSink>();
            serviceCollection.AddSingleton<IMessagePublisherService, LogMessagePublisherService>();

            _serviceProvider = serviceCollection.BuildServiceProvider();

            return _serviceProvider;
        }
        #endregion


    }
}
