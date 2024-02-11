using Kanban.API.Dto.Auth;
using Kanban.API.Mapper;
using Kanban.Application.Interfaces;
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
        private readonly IAuthService _authService;
        public CustomAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IAuthService authService) : base(options, logger, encoder, clock)
        {
            _authService = authService;
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

            var isValidCredentials = TryGetAuthSplit(authrizationHeader, out var authSplit);


            if(!isValidCredentials)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header format"));
            }

            var client = new ClientDto
            {
                Id = authSplit[0],
                Secret = authSplit[1],
            };

            if (!_authService.Login(client.ToApplication()).Result)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid id or secret"));
            }

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(GetClaimsPrincipal(client.Id), Scheme.Name)));
        }

        public static ClaimsPrincipal GetClaimsPrincipal(string id)
        {
            var authClient = new AuthenticationClient
            {
                AuthenticationType = Constants.Authentication,
                IsAuthenticated = true,
                Name = id,
            };

            return new ClaimsPrincipal(new ClaimsIdentity(authClient, new[] { new Claim(ClaimTypes.Name, id), }));
        }

        public static bool TryGetAuthSplit(string authrizationHeader, out string[] authSplit)
        {
            var authBase64Decoded = Encoding.UTF8.GetString(Convert.FromBase64String(authrizationHeader.Replace($"{Constants.Authentication} ", "", StringComparison.OrdinalIgnoreCase)));

            authSplit = authBase64Decoded.Split(new[] { ':' });

            return authSplit.Length == 2 && !string.IsNullOrEmpty(authSplit[0]) && !string.IsNullOrEmpty(authSplit[1]);
        }
    }
}
