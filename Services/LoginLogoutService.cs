using ufshop.Components.UI;
using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
namespace ufshop.Services;

public class LoginLogoutService
{
    public readonly BeService beService;
    
    public LoginLogoutService(BeService srv)
    {
        beService = srv;
    }

    public async Task<bool> TryLogin(string email, string password)
    {
        bool output = false;
        var user = await beService.UserManager.FindByEmailAsync(email);//.Result;
        if (user is not null)
        {
            // beService.SignInManager.SignOutAsync().Wait();
            var lol = await beService.SignInManager.CheckPasswordSignInAsync(user, password, false);
            if (lol.Succeeded)
            {
                // try
                // {
                //     await beService.SignInManager.SignInAsync(user, isPersistent: false);
                // }
                // catch(Exception ex)
                // {
                //     Console.WriteLine(ex.Message);
                // }
                // var casp = new CustomAuthenticationStateProvider();
                // casp.SignIn(user);
                output = true;
            }

            // if (beService.SignInManager.PasswordSignInAsync(user, password, false, false).Result.Succeeded)
            // {
            //     output = true;
            // }
        }
        return output;
    }

    public async Task Logout()
    {
        await beService.SignInManager.SignOutAsync();//.Wait();
    }


}