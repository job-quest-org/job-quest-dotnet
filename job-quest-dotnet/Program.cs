using job_quest_dotnet.JQApiConstants;
using JQ.BusinessLayer;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
//Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddDebug();
// Add services to the container.
var services = builder.Services;
services.AddControllers();
services.AddScoped<CandidateBL>();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

//configure oidc as authorization code flow
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
    options.DefaultSignInScheme = "cookie";
})
    .AddCookie("cookie", options =>
    {
        options.Cookie.Name = "JQ_cookie";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
        options.Cookie.IsEssential = true;
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;//default is Lax even if not provided
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = Environment.GetEnvironmentVariable(JQApiConstants.JQOidcAuthority);
        options.ClientId = Environment.GetEnvironmentVariable(JQApiConstants.JQOidcClientId);
        options.ClientSecret = Environment.GetEnvironmentVariable(JQApiConstants.JQOidcClientSecret); ;
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.CallbackPath = "/signin-oidc";


        options.Events.OnRedirectToIdentityProvider = context =>
        {
            context.ProtocolMessage.RedirectUri = Environment.GetEnvironmentVariable(JQApiConstants.JQOidcRedirectUri);
            return Task.CompletedTask;
        };
        options.Events.OnAuthorizationCodeReceived = context =>
        {
            context.ProtocolMessage.RedirectUri = Environment.GetEnvironmentVariable(JQApiConstants.JQOidcHome);
            context.Response.Redirect(Environment.GetEnvironmentVariable(JQApiConstants.JQOidcHome));
            return Task.CompletedTask;
        };
        options.Events.OnTokenValidated = context =>
        {
            string name = context.Principal?.Claims
                .FirstOrDefault(c => c.Type == "name")?.Value ?? string.Empty;
            string email = context.Principal?.Claims
                .FirstOrDefault(c => c.Type == "email")?.Value ?? string.Empty;
            string picture = context.Principal?.Claims
                .FirstOrDefault(c => c.Type == "picture")?.Value ?? string.Empty;
            IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email),
                new Claim("picture", picture)
            };
            context.Principal?.AddIdentity(new ClaimsIdentity(claims));
            context.ProtocolMessage.RedirectUri = Environment.GetEnvironmentVariable(JQApiConstants.JQOidcHome);
            context.Response.Redirect(Environment.GetEnvironmentVariable(JQApiConstants.JQOidcHome));
            return Task.CompletedTask;
        };

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:5173", "https://localhost:44396") // Add both origins to allow CORS
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseHttpsRedirection();

app.UseAuthentication(); // Add authentication middleware

app.UseAuthorization();

app.MapControllers();

app.Run();
