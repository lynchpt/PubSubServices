using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.IdentityModel.Clients.ActiveDirectory;

using PubSubServices.Data.MessageSink.Interfaces;

namespace PubSubServicesData.MessageSink.ServiceBus
{
    public class DefaultCredentialProvider : ICredentialProvider
    {
        /// <summary>
        /// This method will ONLY return a valid UserCredential for the logged in user account in a domain joined scenario
        /// when Windows Integrated Security is being used. However, this is the default case that we expect both in prod
        /// and pre-pred scenarios.
        /// </summary>
        /// <returns></returns>
        /// 
        /// THIS IS THE REAL IMPLEMENTATION INTENDED FOR NORMAL USE!
        /// UNCOMMENT WHEN FMG ACTIVE DIRECTORY IS SYNCED WITH AZURE AD!!
        /// 
        public UserCredential GetUserCredential()
        {
            return new UserCredential();
        }

        /// THIS IS A HACK IMPLEMENTATION ONLY FOR USE BEFORE FMG ACTIVE DIRECTORY IS SYNCED WITH AZURE AD!!
        /// DELETE WHEN FMG ACTIVE DIRECTORY IS SYNCED WITH AZURE AD!!
        //public UserCredential GetUserCredential()
        //{
        //    //return new UserPasswordCredential("someuser@someuserfmglobal.onmicrosoft.com", "Woco1682");
        //    
        //}
       
    }
}
