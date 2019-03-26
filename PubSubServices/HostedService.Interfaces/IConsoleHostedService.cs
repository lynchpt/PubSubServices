using System;

namespace HostedService.Interfaces
{
    public interface IConsoleHostedService : IDisposable
    {
        //this allows services normally hosted in a Windows Service an entry point
        //that the ServiceHost can rely on to invoke the service when the host is running
        //as a console app
        void StartAsConsole( string[] args );
    }
}
