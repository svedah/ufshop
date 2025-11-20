// using System.Security.Claims;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Components.Authorization;

// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.DependencyInjection;
// using ufshop.Data;

// //add this using line to the component using this provider
// using Microsoft.AspNetCore.Components;


// namespace ufshop.Services;

// public class CustomAuthenticationStateProvider : AuthenticationStateProvider
// {
//     private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

//     public override Task<AuthenticationState> GetAuthenticationStateAsync()
//     {
//         return Task.FromResult(new AuthenticationState(_currentUser));
//     }

//     public void SignIn(ApplicationUser user)
//     {
//         var identity = new ClaimsIdentity(new[]
//         {
//             new Claim(ClaimTypes.Name, user.UserName),
//             new Claim(ClaimTypes.Email, user.Email)
//         }, "CustomAuth");

//         _currentUser = new ClaimsPrincipal(identity);
//         NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
//     }

//     public void SignOut()
//     {
//         _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
//         NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
//     }
// }
