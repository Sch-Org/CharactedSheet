using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace CharacterSheet.IdentityService.Api;

public class CustomGrantTypeValidator : IExtensionGrantValidator
{
    public string GrantType => "custom";

    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        var userId = context.Request.Raw.Get("user_id");

        if(string.IsNullOrEmpty(userId))
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            return;
        }

        context.Result = new GrantValidationResult(
            subject: userId,
            authenticationMethod: "custom",
            claims: new []
            {
                new Claim("user_id", userId)
            }
        );
    }
}
