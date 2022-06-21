using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace FileStorage.Identity
{
    public static class Configuration
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                //new ApiScope("FileStorageWebApi", "Web API")
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName )
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("FileStorageWebAPI", "Web API", new[]
                {
                    JwtClaimTypes.Name
                })
                {
                    Scopes = {"FileStorageWebAPI"}
                }
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "filestorage-web-api",
                    ClientName = "FileStorage Web",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RedirectUris =
                    {
                        "http://localhost:3000/signin-oidc"
                    },
                    AllowedCorsOrigins =
                    {
                        "http://localhost:3000"
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:3000/signout-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "FileStorageWebAPI"
                    },
                    AllowAccessTokensViaBrowser = true
                }
            };
    }
}