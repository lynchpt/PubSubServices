using System;
using System.ServiceProcess;

using HostedService.Interfaces;

using MessagePublisherService;

namespace PubSubServices.PublisherService
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isConsole = Array.IndexOf(args, "--console") > -1 || Environment.CurrentDirectory != Environment.SystemDirectory;

            using (ServiceBase service = new MessagePublisherScheduler())
            {
                if ( isConsole )
                {
                    //we won't be able to run as console unless the ServiceBase instance
                    //also implements IConsoleHostedService, so attempt the cast and signal failure
                    //IConsoleHostedService hostedService = service as IConsoleHostedService;
                    if (service is IConsoleHostedService hostedService )
                    {
                        hostedService.StartAsConsole(args);
  
                        Console.WriteLine($"Service {service.ServiceName} started successfully in console hosted mode");
                        Console.WriteLine();
                        Console.WriteLine($"Press any key to stop {service.ServiceName}.");
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
    }
}
