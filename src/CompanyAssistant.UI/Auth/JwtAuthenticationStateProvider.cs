using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CompanyAssistant.UI.Auth
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _storage;

        public JwtAuthenticationStateProvider(ILocalStorageService storage)
        {
            _storage = storage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var session = await _storage.GetItemAsync<AuthSession>("auth");

            if (session == null)
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));


            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(session.Token);

            var identity = new ClaimsIdentity(jwt.Claims, "jwt", ClaimTypes.Email, ClaimTypes.Role);

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task SignInAsync(string token)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var session = new AuthSession
            {
                Token = token,
                UserId = jwt.Claims.First(x => x.Type == ClaimTypes.NameIdentifier.ToString()).Value,
                Username = jwt.Claims.First(x => x.Type == ClaimTypes.Name.ToString()).Value,
                DisplayName = jwt.Claims.First(x => x.Type == ClaimTypes.GivenName.ToString()).Value,
                Email = jwt.Claims.First(x => x.Type == ClaimTypes.Email.ToString()).Value,
                Roles = jwt.Claims
                    .Where(x => x.Type == ClaimTypes.Role)
                    .Select(x => x.Value)
                    .ToArray()
            };

            await _storage.SetItemAsync("auth", session);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task SignOutAsync()
        {
            await _storage.RemoveItemAsync("authToken");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
