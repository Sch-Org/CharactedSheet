using IdentityServer4.Models;

namespace CharacterSheet.IdentityService.Api.Config;

public static class IdentityConfig
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("gateway", "Api Gateway")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client 
            {
                ClientId = "client",
                AllowedGrantTypes = new List<string> { GrantType.ClientCredentials, "refresh_token" },
                ClientSecrets = 
                {
                    new Secret("client_secret".Sha256())
                },
                AllowedScopes = { "gateway" }
            }
        };
}
