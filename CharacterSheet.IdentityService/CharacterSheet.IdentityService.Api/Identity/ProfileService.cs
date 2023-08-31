using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace CharacterSheet.IdentityService.Api;

public class ProfileService : IProfileService
{
    private readonly UserManager<IdentityUser> _userManager;

    public ProfileService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        var claims = new List<Claim>
        {
            new Claim("email", user.UserName)
        };

        context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);

        context.IsActive = user != null;
    }
}
