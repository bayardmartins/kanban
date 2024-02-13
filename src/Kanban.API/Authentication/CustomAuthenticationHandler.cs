using Kanban.Model.Dto.API.Auth;
using Kanban.Model.Mapper.Auth;
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

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(Constants.Authorization))
            {
                Context.Response.Body = new MemoryStream(Encoding.UTF8.GetBytes(Constants.MissingAuthorizationKey));
                return AuthenticateResult.Fail(Constants.MissingAuthorizationKey);
            }

            var authrizationHeader = Request.Headers[Constants.Authorization].ToString();

            if (!authrizationHeader.StartsWith(Constants.Authentication, StringComparison.OrdinalIgnoreCase))
            {
                Context.Response.Body = new MemoryStream(Encoding.UTF8.GetBytes(Constants.AuthorizationHeaderMalformed));
                return AuthenticateResult.Fail(Constants.AuthorizationHeaderMalformed);
            }

            var isValidCredentials = TryGetAuthSplit(authrizationHeader, out var authSplit);


            if(!isValidCredentials)
            {
                Context.Response.Body = new MemoryStream(Encoding.UTF8.GetBytes(Constants.InvalidAuthorizationHeaderFormat));
                return AuthenticateResult.Fail(Constants.InvalidAuthorizationHeaderFormat);
            }

            var client = new ClientDto
            {
                Id = authSplit[0],
                Secret = authSplit[1],
            };

            if (!_authService.Login(client.ToApplication()).Result)
            {
                Context.Response.Body = new MemoryStream(Encoding.UTF8.GetBytes(Constants.InvalidIdOrSecret));
                return AuthenticateResult.Fail(Constants.InvalidIdOrSecret);
            }

            return AuthenticateResult.Success(new AuthenticationTicket(GetClaimsPrincipal(client.Id), Scheme.Name));
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
