using System;
using System.ServiceProcess;

namespace PubSubServices.PublisherService
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isConsole = Array.IndexOf(args, "--console") > -1 || Environment.CurrentDirectory != Environment.SystemDirectory;

            using (var service = new PublisherService())
            {
                if ( isConsole )
                {
                    service.StartAsConsole(args);
                    Console.WriteLine("Press any key to stop program.");
                    Console.ReadLine();
                    service.Stop();
                }
                else
                {
                    ServiceBase.Run(service);
                }
            }
        }
    }
}
