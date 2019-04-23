using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PubSubServices.Data.MessageSink.Interfaces
{
    public interface IConnectionInfoProvider
    {
        Task<string> GetConnectionStringAsync(
            string connectionStringStore,
            string authorizationStore,
            string connectionStringName,
            ICredentialProvider credentialProvider );
    }
}
