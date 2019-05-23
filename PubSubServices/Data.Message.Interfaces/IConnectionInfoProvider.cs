using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PubSubServices.Data.Message.Interfaces
{
    public interface IConnectionInfoProvider
    {
        Task<string> GetConnectionStringAsync();
    }
}
