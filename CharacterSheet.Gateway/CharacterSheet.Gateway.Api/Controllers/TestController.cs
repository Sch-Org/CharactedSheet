using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharacterSheet.Gateway.Api;

[Route("api/test")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TestController : ControllerBase
{
    [HttpGet("test")]
    public string Test()
    {
        return "SUCCESS";
    }
}
