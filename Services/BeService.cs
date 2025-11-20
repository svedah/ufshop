
using Microsoft.AspNetCore.Identity;
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
    public readonly string wwwroot;

    public readonly string DomainPrefix;

    public BeService
    (
        ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment iWebHostEnvironment
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
        // IdentityRedirectManager = identityRedirectManager;
        // ServiceProvider = serviceProvider;


        wwwroot = iWebHostEnvironment.WebRootPath + Path.DirectorySeparatorChar;
        DomainPrefix = DomainHelper.ExtractSubDomain(httpContextAccessor);
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
