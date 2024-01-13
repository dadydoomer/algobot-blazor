using CryptoExchange.Net.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace BlazorAlgoBot.Server.Authorization
{
    public class ApiKeyHandler : AuthenticationHandler<ApiKeySchemeOptions>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiKeyHandler(IOptionsMonitor<ApiKeySchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IHttpContextAccessor httpContextAccessor) : base(options, logger, encoder, clock)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            StringValues apiKeys = StringValues.Empty;
            bool apiKeyPresent = _httpContextAccessor.HttpContext?.Request.Headers.TryGetValue(ApiKeySchemeOptions.HeaderName, out apiKeys) ?? false;

            if (!apiKeyPresent)
            {
                return AuthenticateResult.NoResult();
            }

            if (apiKeys.Count > 1)
            {
                return AuthenticateResult.Fail("Multiple API keys found in request. Please only provide one key.");
            }

            if (string.IsNullOrEmpty(Options.ApiKey) || !Options.ApiKey.Equals(apiKeys.FirstOrDefault()))
            {
                return AuthenticateResult.Fail("Invalid API key.");
            }

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, Options.ApiKey),
            };
            ClaimsIdentity identity = new(claims, Scheme.Name);
            ClaimsPrincipal principal = new(identity);
            AuthenticationTicket ticket = new(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
