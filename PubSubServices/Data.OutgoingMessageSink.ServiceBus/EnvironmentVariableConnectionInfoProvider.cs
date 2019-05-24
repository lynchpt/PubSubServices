using Microsoft.Extensions.Options;
using PubSubServices.Data.Message.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PubSubServicesData.MessageSink.ServiceBus
{
    public class EnvironmentVariableConnectionInfoProvider : IConnectionInfoProvider
    {
        #region Class Variables
        ServiceBusEnvVarConnectionInfoOptions _connectionInfoOptions;
        #endregion

        #region Constructors
        public EnvironmentVariableConnectionInfoProvider(IOptions<ServiceBusEnvVarConnectionInfoOptions> optionsAccessor)
        {
            _connectionInfoOptions = optionsAccessor?.Value;
        }
        #endregion

        public async Task<string> GetConnectionStringAsync()
        {
            string connectionStringName = _connectionInfoOptions.ConnectionStringName;
            string connectionString = Environment.GetEnvironmentVariable(connectionStringName, EnvironmentVariableTarget.Machine);

            return await Task.FromResult<string>(connectionString);
        }

        public async Task<string> GetConnectionStringAsync(string connectionStringStore, string authorizationStore, string connectionStringName, ICredentialProvider credentialProvider)
        {
            string connectionString = await GetConnectionStringAsync();
            return connectionString;
        }

    }
}
