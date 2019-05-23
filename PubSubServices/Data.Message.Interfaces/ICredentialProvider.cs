using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace PubSubServices.Data.Message.Interfaces
{
    public interface ICredentialProvider
    {
        UserCredential GetUserCredential();
    }
}
