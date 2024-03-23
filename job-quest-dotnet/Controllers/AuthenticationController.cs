using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
namespace JQ.Controllers
{
    [ApiController]
    [Route("login")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Login()
        {
            try
            {

                return Challenge(new AuthenticationProperties { RedirectUri = "https://accounts.google.com/", IsPersistent = true, }, "oidc");
            }
            catch (OpenIdConnectProtocolException ex)
            {
                _logger.LogError(ex, "An error occurred while handling the remote login");
                throw;
            }
        }

    }
}
