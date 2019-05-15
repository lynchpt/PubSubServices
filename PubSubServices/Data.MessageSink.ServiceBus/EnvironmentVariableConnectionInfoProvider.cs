using Microsoft.Extensions.Options;
using PubSubServices.Data.MessageSink.Interfaces;
using PubSubServices.Model.MessageSink.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PubSubServicesData.MessageSink.ServiceBus
{
    public class EnvironmentVariableConnectionInfoProvider : IConnectionInfoProvider
    {
        #region Class Variables
        EnvironmentVariableConnectionInfoOptions _connectionInfoOptions;
        #endregion

        #region Constructors
        public EnvironmentVariableConnectionInfoProvider(IOptions<EnvironmentVariableConnectionInfoOptions> optionsAccessor)
        {
            _connectionInfoOptions = optionsAccessor?.Value;
        }
        #endregion

        public Task<string> GetConnectionStringAsync()
        {
            string connectionStringName = _connectionInfoOptions.ConnectionStringName;
            string connectionString = Environment.GetEnvironmentVariable(connectionStringName, EnvironmentVariableTarget.Machine);

            return Task.FromResult<string>(connectionString);
        }

        public Task<string> GetConnectionStringAsync(string connectionStringStore, string authorizationStore, string connectionStringName, ICredentialProvider credentialProvider)
        {
            throw new NotImplementedException();
        }

    }
}
