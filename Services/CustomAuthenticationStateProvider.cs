using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using StockControl.Models;
using System.Security.Claims;

namespace StockControl.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            throw new NotImplementedException();
        }

        private readonly ProtectedSessionStorage _sessionStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        //public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    try
        //    {
        //        var userSessionStorageResult = await _sessionStorage.GetAsync<User>("User");
        //        var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;

        //        if (userSession == null)
        //            return await Task.FromResult(new AuthenticationState(_anonymous));

        //        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Email, userSession.Email),
        //            new Claim(ClaimTypes.Role, userSession.Role.Name)
        //        }, "CustomAuth"));
        //        return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        //    }
        //    catch 
        //    {
        //        return await Task.FromResult(new AuthenticationState(_anonymous));
        //    }
        //}

        public async Task UpdateAuthenticationState(User user)
        {
            ClaimsPrincipal claimsPrincipal;

            if (user != null)
            {
                await _sessionStorage.SetAsync("User", user);
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role , user.Role.Name)
                }));
            }
            else
            {
                await _sessionStorage.DeleteAsync("User");
                claimsPrincipal = _anonymous;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
