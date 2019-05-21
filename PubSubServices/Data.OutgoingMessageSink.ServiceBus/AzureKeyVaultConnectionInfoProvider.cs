using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

using PubSubServices.Data.MessageSink.Interfaces;


namespace PubSubServicesData.MessageSink.ServiceBus
{
    /// <summary>
    /// This is a sample implementation of a helper class that encapsulates the steps needed to get a connection string for
    /// Azure Service Bus from Azure Key Vault (where the connection string is treated as a "secret"). 
    /// These connection strings have sensitive data (the actual key that allows callers to publish or recieve messages from Topics 
    /// in a Namespace) embedded in them, and are thus not suitable for storage in a config file on an app server. Azure Key Vault 
    /// ensures that only authorized users (typically an app server service account) can request a connection string.
    /// 
    /// Calling Azure Key Vault has several steps that are not immediately obvious, so this sample shows you how to make the call, and 
    /// provides a starting point for your own implementation.
    /// </summary>
    public class AzureKeyVaultConnectionInfoProvider : IConnectionInfoProvider
    {
        #region Class Variables
        //
        //Getting a connection string from Azure Key Vault using a token provided by an Azure Active Directory application requires 
        //a UserCredential object. In normal operation (prod and pre-prod), the identity of the running process will be used to create that
        //UserCredential. Code such as userCredential = new UserCredential() would be sufficient and such code can be included directly in a 
        //helper class such as this one.
        //
        //This helper class implementation accomodates non-standard development scenarios by using a helper model where calling code
        //is responsible for "somehow" embedding into the Provider a UserCredential object that does not have the identity of the running process. 
        //This will help in cases where the logged in user does not have their account synchronized to the appropriate Azure Active Directory, or
        //if the account of the logged in user is not a member of one of the AD Groups that is granted access to Azure Key Vault.
        //This synthesized UserCredential object must be for an account that is granted such access. 
        private UserCredential _userCredential;

        //This identifier (typically a guid) represents the FM Global Azure Active Directory Application that has been 
        //configured to allow access to the Azure Key Vault service for those users who can successfully authenticate to this Azure AD
        //application. In normal usage (prod and pre-prod), the FM Global Active Directory will be synced with Azure Active Directory,
        //thus all FM Global users should be able to successfully obtain a token alllowing them to call to various Azure Services (including
        //Key Vault). The Key Vault itself will be configured with permissions about what each caller can actually do (e.g. read secrets, write new
        //secrets, etc.). Each Key Vault contains a certain set of named secrets and is set up to allow a set of 
        //authorized users or groups to request the value of a particular named secret. 
        //
        //This application identifier is not security sensitive and can be stored in a config file
        private string _azureADApplicationId;

        private ILogger<AzureKeyVaultConnectionInfoProvider> _logger;
        KeyVaultConnectionInfoOptions _connectionInfoOptions;
        private ICredentialProvider _credentialProvider;
        #endregion

        #region Constructors
        public AzureKeyVaultConnectionInfoProvider(ILogger<AzureKeyVaultConnectionInfoProvider> logger, 
            IOptions<KeyVaultConnectionInfoOptions> optionsAccessor, ICredentialProvider credentialProvider)
        {
            _logger = logger;
            _connectionInfoOptions = optionsAccessor?.Value;
            _credentialProvider = credentialProvider;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyVaultUrl">the url where the particular Key Vault instance can be accessed. 
        /// This value is not security sensitive and can be stored in a config file by the caller.</param>
        /// <param name="azureADApplicationId">This identifier (typically a guid) represents an "application" that has been configured on a specific Key Vault.
        ///  This value is not security sensitive and can be stored in a config file by the caller.</param>
        /// <param name="secretName">The string designating which secret you wish to retrieve (so as to inspect the secret value). 
        /// This value is not security sensitive and can be stored in a config file by the caller.</param>
        /// <param name="credentialProvider">a class implementing the sample ICredentialProvider interface. The caller provides
        /// the particular implementation in which they have loaded the UserCredential object they got from somewhere.</param>
        /// <returns></returns>
        public async Task<string> GetAzureServiceBusConnectionStringAsync(string keyVaultUrl, string azureADApplicationId, string secretName,
            ICredentialProvider credentialProvider)
        {

            _azureADApplicationId = azureADApplicationId;

            //Get UserCredential needed to talk to Azure Key Vault to ask for the connection string
            _userCredential = credentialProvider.GetUserCredential();


            KeyVaultClient keyVaultClient = GetKeyVaultClient();


            var secret = await keyVaultClient.GetSecretAsync(keyVaultUrl, secretName);

            string connectionString = secret.Value;

            return connectionString;
        }

        private KeyVaultClient GetKeyVaultClient()
        {
            KeyVaultClient keyVaultClient = new KeyVaultClient(AuthenticationCallback);

            return keyVaultClient;
        }

        private async Task<string> AuthenticationCallback(string authority, string resource, string scope) //scope not used
        {
            AuthenticationContext authenticationContext = new AuthenticationContext(authority, TokenCache.DefaultShared);

            string token = await GetAzureKeyVaultAccessTokenAsync(authenticationContext, _userCredential, resource, _azureADApplicationId);

            return token;
        }


        private async Task<string> GetAzureKeyVaultAccessTokenAsync(
            AuthenticationContext authenticationContext,
            UserCredential userCredential,
            string resourceId,
            string clientId) //scope
        {
            AuthenticationResult result = null;

            // first, try to get a token silently
            try
            {
                result = authenticationContext.AcquireTokenSilentAsync(resourceId, clientId).Result;
            }
            catch (AggregateException exc)
            {
                AdalException ex = exc.InnerException as AdalException;

                // There is no token in the cache; prompt the user to sign-in.
                if (ex != null && ex.ErrorCode != "failed_to_acquire_token_silently") //this is an expected error
                {
                    // An unexpected error occurred.
                    throw ex;
                }
            }

            //try to acquire token by explicitly providing credentials
            if (result == null)
            {
                result = await authenticationContext.AcquireTokenAsync(resourceId, clientId, userCredential);
            }

            return result.AccessToken;
        }

        #region IConnectionInfoProvider Implementation

        public Task<string> GetConnectionStringAsync(
            string connectionStringStore,
            string authorizationStore,
            string connectionStringName,
            ICredentialProvider credentialProvider)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetConnectionStringAsync()
        {
            string connectionString = await GetAzureServiceBusConnectionStringAsync(
                _connectionInfoOptions.ConnectionStringStore,
                _connectionInfoOptions.AuthorizationStore,
                _connectionInfoOptions.ConnectionStringName,
                _credentialProvider);

            return connectionString;
        } 
        #endregion
    }
}
