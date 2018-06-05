using Bit.IdentityServer.Contracts;
using Bit.IdentityServer.Implementations;
using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;

namespace Sanaap.Api.Implementations.Security
{
    public class SanaapOAuthClientsProvider : OAuthClientsProvider
    {
        public override IEnumerable<Client> GetClients()
        {
            return new[]
            {
                GetResourceOwnerFlowClient(new BitResourceOwnerFlowClient
                {
                    ClientName = "SanaapResOwner",
                    ClientId = "SanaapResOwner",
                    Secret = "secret",
                    TokensLifetime = TimeSpan.FromDays(9999),
                    Enabled = true
                }),
                GetResourceOwnerFlowClient(new BitResourceOwnerFlowClient
                {
                    ClientName = "SanaapOperatorAppResOwner",
                    ClientId = "SanaapOperatorAppResOwner",
                    Secret = "secret",
                    TokensLifetime = TimeSpan.FromDays(9999),
                    Enabled = true
                })
            };
        }
    }
}
