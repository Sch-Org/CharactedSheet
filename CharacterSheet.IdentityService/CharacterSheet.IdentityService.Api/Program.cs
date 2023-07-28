using CharacterSheet.IdentityService.Api.Config;
using CharacterSheet.IdentityService.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<IdentityContext>(config => 
{
    config.UseInMemoryDatabase("IdentityDb");
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(config =>
{
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireDigit = false;
    config.Password.RequireUppercase = false;
    config.Password.RequiredLength = 1;
})
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddDeveloperSigningCredential()
    .AddInMemoryApiScopes(IdentityConfig.ApiScopes)
    .AddInMemoryClients(IdentityConfig.Clients);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => 
    {
        options.Authority = "http://localhost:5001";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidIssuer = "http://localhost:5001"
        };
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "gateway");
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseIdentityServer();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
