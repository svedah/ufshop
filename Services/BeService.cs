
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using ufshop.Data;
using ufshop.Helpers;
namespace ufshop.Services;

public class BeService
{
    public readonly ApplicationDbContext DbContext;
    public readonly UserManager<ApplicationUser> UserManager;
    public readonly RoleManager<IdentityRole> RoleManager;
    public readonly SignInManager<ApplicationUser> SignInManager;
    public readonly IHttpContextAccessor HttpContextAccessor;
    // public readonly IServiceProvider ServiceProvider;
    public readonly IWebHostEnvironment WebHostEnvironment;
    // public readonly IdentityRedirectManager IdentityRedirectManager;
    public readonly IJSRuntime JsRuntime;
    
    // public readonly AuthenticationStateProvider AuthenticationStateProvider;
    // public readonly AuthenticationState AuthenticationState;
    public readonly string wwwroot;

    public string DomainPrefix
    {
        get
        {
            var domainExtract = new DomainExtract(HttpContextAccessor.HttpContext.Request.Host.Host);
            return domainExtract.Prefix;
        }
    }
    

    public BeService
    (
        ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        AuthenticationStateProvider authenticationStateProvider,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment iWebHostEnvironment,
        IJSRuntime iJSRunTime
        // IdentityRedirectManager identityRedirectManager
    // IServiceProvider serviceProvider
    )
    {
        DbContext = dbContext;
        UserManager = userManager;
        SignInManager = signInManager;
        RoleManager = roleManager;
        HttpContextAccessor = httpContextAccessor;
        WebHostEnvironment = iWebHostEnvironment;
        JsRuntime = iJSRunTime;

        // AuthenticationStateProvider = authenticationStateProvider;
        // AuthenticationState = AuthenticationStateProvider.GetAuthenticationStateAsync().Result;


        // IdentityRedirectManager = identityRedirectManager;
        // ServiceProvider = serviceProvider;


        wwwroot = iWebHostEnvironment.WebRootPath + Path.DirectorySeparatorChar;
        // DomainPrefix = DomainHelper.ExtractSubDomain(httpContextAccessor);
    }

    //var context = ServiceProvider.GetService<ApplicationDbContext>();
    
    public string SetSaneValues()
    {
        //kolla att session har de värden resten av applikationen förväntar sig
        //cartid
        //shopid
        //customerid
        return string.Empty;
    }

}
