using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Providers
{
    public class CustomAuthValidateProvider: RevalidatingServerAuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage localStorage;
        private AuthenticationState currentAuthState;
        public CustomAuthValidateProvider(ILoggerFactory loggerFactory,
            ProtectedLocalStorage localStorage) : base(loggerFactory)
        {
            this.localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            AuthenticationState CreateAnonymous()
            {
                var anonymousPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
                return new AuthenticationState(anonymousPrincipal);
            }

            var securityToken = (await localStorage.GetAsync<SecurityTokenViewModel>(nameof(SecurityTokenViewModel))).Value;
            if (!IsValidToken(securityToken)) return CreateAnonymous();

            var tokenSecurity = new JwtSecurityToken(securityToken.AccessToken);
            var identity = new ClaimsIdentity(tokenSecurity.Claims, "Bearer");
            var principal = new ClaimsPrincipal(identity);
            var successAuthState = new AuthenticationState(principal);
            NotifyAuthenticationStateChanged(Task.FromResult(successAuthState));
            return successAuthState;
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(10);

        protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            var securityToken = (await localStorage.GetAsync<SecurityTokenViewModel>(nameof(SecurityTokenViewModel))).Value;
            return IsValidToken(securityToken);
        }

        public async Task MakeUserAnonymous()
        {
            await localStorage.DeleteAsync(nameof(SecurityTokenViewModel));
            var anonymousPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousPrincipal));
            NotifyAuthenticationStateChanged(authState);
        }

        protected bool IsValidToken(SecurityTokenViewModel? securityToken)
        {
            if (securityToken is null) return false;

            if (string.IsNullOrEmpty(securityToken.AccessToken) || securityToken.ExpiredAt < DateTime.Now)
            {
                return false;
            }
            return true;
        }
    }
}
