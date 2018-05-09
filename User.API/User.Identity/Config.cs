using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity
{
    public class Config
    {
        public static IEnumerable<Client> GetClients()
        {
            List<Client> clients = new List<Client>();
            Client client = new Client()
            {
                ClientId = "dellpc",
                ClientSecrets = { new Secret("yemobai".Sha256()) },
                RefreshTokenExpiration = TokenExpiration.Sliding,
                AllowOfflineAccess = true,
                RequireClientSecret = false,
                //AllowedGrantTypes=
                AllowedGrantTypes = new List<string> { "sms_auth_code" },
                //会直接返回所有的claims的信息
                AlwaysIncludeUserClaimsInIdToken = true,            
                AllowedScopes = new List<string>  {
                    "gateway_api",
                     "user_api",
                     "contact_api",
                    IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                    IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess
                    }
            };
            Client client2 = new Client()
            {
                ClientId = "pc",
                ClientSecrets = { new Secret("yemobai".Sha256()) },
                RefreshTokenExpiration = TokenExpiration.Sliding,
                AllowOfflineAccess = true,
                RequireClientSecret = false,
                //AllowedGrantTypes=
                AllowedGrantTypes = new List<string> { "sms_auth_code" },
                //会直接返回所有的claims的信息
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowedScopes = new List<string>  {
                    "gateway_api",
                    "contact_api",
                    "user_api",
                    IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                    IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess
                    }
            };
            clients.Add(client);
            clients.Add(client2);
            return clients;
        }
        public static IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource>
            {
                new ApiResource("gateway_api","gateway service"),

                new ApiResource("user_api","user_api service"),
                //并且要把contactapi加入到apiResource,并加入到 client的allowedScopes中 
                new ApiResource("contact_api","contact_api service")
            };
        }
        public static List<TestUser> GetTestUser()
        {
            return new List<TestUser>
            {
                new TestUser(){SubjectId="10000",Username="yemobai",Password="password",}

            };
        }
        public static IEnumerable<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>
             {
                 new IdentityResources.OpenId(),
                 new IdentityResources.Profile(),
                 new IdentityResources.Email()
             };
        }
    }
}
