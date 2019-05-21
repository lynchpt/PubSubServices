using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace PubSubServices.Data.MessageSink.Interfaces
{
    public interface ICredentialProvider
    {
        UserCredential GetUserCredential();
    }
}
