using CharacterSheet.IdentityService.Api.Config;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CharacterSheet.IdentityService.Api.Controllers;

[Route("api/account")]
public class AccountController : ControllerBase
{
    public class SignInRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    [HttpGet("test")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public string Test()
    {
        return "SUCCESS";
    }

    [HttpPost("signup")]
    public async Task<string> SignUp([FromBody] SignInRequest request, [FromServices] UserManager<IdentityUser> userManager)
    {
        try
        {
            var created = await userManager.CreateAsync(new IdentityUser(request.Login)
            {
                EmailConfirmed = true,
                Email = request.Login
            });

            if (!created.Succeeded)
            {
                return string.Join(", ", created.Errors.Select(e => e.Description).ToList());
            }

            var user = await userManager.FindByEmailAsync(request.Login);
            var addedPassword = await userManager.AddPasswordAsync(user, request.Password);

            if (!addedPassword.Succeeded)
            {
                return string.Join(", ", addedPassword.Errors.Select(e => e.Description).ToList());
            }
            return "SUCCESS";
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    [HttpPost("signin")]
    public async Task<string> SignIn([FromBody] SignInRequest request, [FromServices] UserManager<IdentityUser> userManager)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(request.Login);
            if (user != null)
            {
                var passwordValidation = await userManager.CheckPasswordAsync(user, request.Password);
                if (passwordValidation)
                {
                    using (var httpClient = new HttpClient())
                    {
                        var client = IdentityConfig.Clients.FirstOrDefault();
                        httpClient.BaseAddress = new Uri("http://localhost:5001");
                        var requestContent = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("client_id", client.ClientId),
                            new KeyValuePair<string, string>("client_secret", "client_secret"),
                            new KeyValuePair<string, string>("scope", "gateway"),
                            new KeyValuePair<string, string>("grant_type", GrantType.ClientCredentials)
                        };
                        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "connect/token")
                        {
                            Content = new FormUrlEncodedContent(requestContent)
                        };
                        var result = await httpClient.SendAsync(requestMessage);

                        return $"{result?.StatusCode}: {await result?.Content?.ReadAsStringAsync()}";

                    }
                }
                else
                {
                    return "Wrong password";
                }
            }
            else
            {
                return $"No user found for {request.Login}";
            }
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}
