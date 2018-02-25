using Bit.IdentityServer.Contracts;
using Bit.IdentityServer.Implementations;
using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;

namespace Sanaap.Api.Implementations.Security
{
    public class SanaapClientProvider : ClientProvider
    {
        public override IEnumerable<Client> GetClients()
        {
            return new[]
            {
                GetResourceOwnerFlowClient(new BitResourceOwnerFlowClient
                {
                    //ClientId = "Sanaap-ResOwner",
                    //ClientName = "Sanaap-ResOwner",
                    //Secret = "secret",
                    //TokensLifetime = TimeSpan.FromDays(7)

                    ClientId = "1",
                    ClientName = "1",
                    Secret = "1",
                    TokensLifetime = TimeSpan.FromDays(9999)
                })
            };
        }
    }
}
