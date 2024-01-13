using CryptoExchange.Net.CommonObjects;
using Microsoft.AspNetCore.Components.Authorization;
using System.Dynamic;
using System.Security.Claims;

namespace Algobot.Client.Authentication
{
    public class PasswordOnlyAuthenticationStateProvider : AuthenticationStateProvider
    {
        private const string UserId = "1";

        private static ClaimsPrincipal Anonymous => new ClaimsPrincipal(new ClaimsIdentity(Array.Empty<Claim>(), string.Empty));
        private static ClaimsPrincipal LoggedIn => new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim("Id", UserId) }, "PasswordOnly"));


        private static AuthenticationState _authenticationState = new AuthenticationState(Anonymous);
        private readonly HttpClient _httpClient;

        public PasswordOnlyAuthenticationStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(_authenticationState);
        }

        public async Task AuthenticateUser(string password)
        {
            var loginSuccess = await Login(password);

            if (loginSuccess)
            {
                _authenticationState = new AuthenticationState(LoggedIn);
            }

            NotifyAuthenticationStateChanged(
                Task.FromResult(_authenticationState));
        }

        private async Task<bool> Login(string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentException("Password cannot be empty.");
                }

                var response = await _httpClient.GetAsync($"User/{password}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
