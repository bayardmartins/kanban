using Kanban.CrossCutting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Kanban.API.Authentication
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public CustomAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Key"));
            }

            var authrizationHeader = Request.Headers["Authorization"].ToString();

            if (!authrizationHeader.StartsWith(Constants.Authentication, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization header mal formed"));
            }

            var authBase64Decoded = Encoding.UTF8.GetString(Convert.FromBase64String(authrizationHeader.Replace($"{Constants.Authentication} ", "", StringComparison.OrdinalIgnoreCase)));

            var authSplit = authBase64Decoded.Split(new[] {  ':'}, 2);

            if(authSplit.Length != 2)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header format"));
            }

            var clientId = authSplit[0];
            var clientSecret = authSplit[1];

            // TODO: fetch in database
            if (clientId == null || clientSecret == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid id or secret"));
            }

            var client = new AuthenticationClient
            {
                AuthenticationType = Constants.Authentication,
                IsAuthenticated = true,
                Name = clientId,
            };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(client, new[]
            {
                new Claim(ClaimTypes.Name, clientId),
            }));

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
        }
    }
}
