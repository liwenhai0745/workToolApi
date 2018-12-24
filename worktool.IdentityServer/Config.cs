using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace worktool.IdentityServer
{
    public class Config
    {

        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        // scopes define the API resources in your system
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("workToolApi", "workToolApi")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                //new Client
                //{
                //    ClientId = "client",
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,

                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    AllowedScopes = { "api1" }
                //},

                // resource owner password grant client
                new Client
                {
                    ClientId = "worktool.pc",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AccessTokenLifetime=3600*72,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "workToolApi" }
                },
                  // JavaScript Client
                new Client
                {
                    ClientId = "worktool.js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime=3600*72,
                    RedirectUris = { "http://192.168.23.9:8102/static/callback.html" },
                    PostLogoutRedirectUris = { "http://192.168.23.9:8102/index.html" },
                    AllowedCorsOrigins = { "http://192.168.23.9:8102" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "workToolApi"
                    },
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                } 
                ,new TestUser
                {
                    SubjectId = "3",
                    Username = "lwh",
                    Password = "lwh"
                },
            };
        }
    }
}
