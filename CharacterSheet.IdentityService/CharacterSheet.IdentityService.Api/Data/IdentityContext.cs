using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CharacterSheet.IdentityService.Api.Data;

public class IdentityContext : IdentityDbContext<IdentityUser>
{
    public IdentityContext(DbContextOptions<IdentityContext> options)  
        : base(options)  
    {  
    }
}
