using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using ufshop.Data.Models;
using ufshop.Services;
using ufshop.Shared;

namespace ufshop.Data.Seed;

public class Seeder
{
    // private readonly ApplicationDbContext DbContext;
    readonly BeService BeService;
    public Seeder(BeService beService)
    // public Seeder(ApplicationDbContext dbContext)
    {
        BeService = beService;
        // DbContext = dbContext;
    }

    public void Seed()
    {
        ApplicationUser adminUser;
        ApplicationUser shopIsacUser;
        ApplicationUser shopJojeUser;

        SeedRolesAndUsers(out adminUser, out shopIsacUser, out shopJojeUser);

        if (!BeService.DbContext.Set<Shop>().Any())
        {
            if (adminUser is null || shopIsacUser is null || shopJojeUser is null)
            {
                throw new Exception("Default users are null after seeding");
            }
            SeedShops(adminUser, shopIsacUser, shopJojeUser);
        }

        SeedEmptyImage();
    }

    void SeedEmptyImage()
    {
        if (!BeService.DbContext.ShopImages.Where(e => e.Filename.Equals(Constants.EMPTYIMAGEFILENAME)).Any())
        {
            ShopImage shopImage = new ShopImage
            {
                Id = Constants.EMPTYIMAGEGUID, //BUG: (ef core bug) använd EJ Guid.Empty, den blir NewGuid() i databasen
                AltText = "Tom bild",
                Created = DateTime.UnixEpoch,
                Filename = Constants.EMPTYIMAGEFILENAME
            };
            BeService.DbContext.Set<ShopImage>().Add(shopImage);
            BeService.DbContext.SaveChanges();
        }
    }

    void SeedShops(ApplicationUser adminUser, ApplicationUser shopIsacUser, ApplicationUser shopJojeUser)
    {
        if (!BeService.DbContext.Set<Shop>().Any())
        {
            var wwwshop = new SeederWWWShop(BeService.DbContext);
            wwwshop.Seed(adminUser);

            var isacShop = new SeederIsacShop(BeService.DbContext);
            isacShop.Seed(shopIsacUser);

            var jojeShop = new SeederJojeShop(BeService.DbContext);
            jojeShop.Seed(shopJojeUser);
        
        }
    }

    void SeedIsacShop(ApplicationUser user)
    {
        var isacshop = new SeederIsacShop(BeService.DbContext);
        isacshop.Seed(user);
    }

    void SeedJojeShop(ApplicationUser user)
    {
        var isacshop = new SeederIsacShop(BeService.DbContext);
        isacshop.Seed(user);
    }


    private async Task SeedIdentityRoles()
    {
        if (!BeService.DbContext.Roles.Any())
        {
            var roles = new string[]{
                "Administrator",
                "Owner"
            };

            foreach (string role in roles)
            {
                await BeService.RoleManager.CreateAsync(new IdentityRole(role));
                // var roleStore = new RoleStore<IdentityRole>(_srv.DbContext);
                // roleStore.AutoSaveChanges = true;
                // if (!_srv.DbContext.Roles.Any(x => x.Name == role))
                // {
                //     roleStore.CreateAsync(new IdentityRole(role)).Wait();
                // }
            }
            await BeService.DbContext.SaveChangesAsync();
        }
    }

    private void SeedRolesAndUsers(out ApplicationUser adminUser, out ApplicationUser shopIsacUser, out ApplicationUser shopJojeUser)//Shop WWW, Shop Isac)
    {
        SeedIdentityRoles().Wait();

        if (!BeService.DbContext.Set<ApplicationUser>().Any())
        {
            adminUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                // Shop = null,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                UserName = "lara@" + Shared.Constants.DOMAINNAME,
                Email = "lara@" + Shared.Constants.DOMAINNAME,
                NormalizedEmail = "LARA@" + Shared.Constants.DOMAINNAME.ToUpper(),
                NormalizedUserName = "LARA@" + Shared.Constants.DOMAINNAME.ToUpper(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
            };

            shopIsacUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                // Shop = shop,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                UserName = "isac@" + Shared.Constants.DOMAINNAME,
                Email = "isac@" + Shared.Constants.DOMAINNAME,
                NormalizedEmail = "ISAC@" + Shared.Constants.DOMAINNAME.ToUpper(),
                NormalizedUserName = "ISAC@" + Shared.Constants.DOMAINNAME.ToUpper(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
            };

            shopJojeUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                // Shop = shop,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                UserName = "joje@" + Shared.Constants.DOMAINNAME,
                Email = "joje@" + Shared.Constants.DOMAINNAME,
                NormalizedEmail = "JOJE@" + Shared.Constants.DOMAINNAME.ToUpper(),
                NormalizedUserName = "JOJE@" + Shared.Constants.DOMAINNAME.ToUpper(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
            };

            SeedUser(adminUser, "lara1234", "Administrator");
            SeedUser(shopIsacUser, "isac1234", "Owner");
            SeedUser(shopJojeUser, "joje1234", "Owner");
        }
        // adminUser = BeService.DbContext.Users.Where(e => e.UserName == "lara@webbhelp.se").First();
        // shopUser = BeService.DbContext.Users.Where(e => e.UserName == "isac@webbhelp.se").First();

        if (BeService.UserManager.FindByEmailAsync("lara@ufshop.nu").Result is null ||
            BeService.UserManager.FindByEmailAsync("isac@ufshop.nu").Result is null ||
            BeService.UserManager.FindByEmailAsync("joje@ufshop.nu").Result is null)
        {
            throw new Exception("Default users lara/isac/joje is null");
        }

        adminUser = BeService.UserManager.FindByEmailAsync("lara@ufshop.nu").Result!;
        shopIsacUser = BeService.UserManager.FindByEmailAsync("isac@ufshop.nu").Result!;
        shopJojeUser = BeService.UserManager.FindByEmailAsync("joje@ufshop.nu").Result!;
    }

    private void SeedUser(ApplicationUser user, string password, string role)
    {
        Contract.Assert(user is not null);
        Contract.Assert(user.Email is not null);

        bool userExists = BeService.DbContext.Users.Any(e => e.UserName == user.UserName);
        if (!userExists)
        {
            user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, password);

            var userStore = new UserStore<ApplicationUser>(BeService.DbContext);
            userStore.CreateAsync(user).Wait();

            //role
            var gotUser = BeService.UserManager.FindByEmailAsync(user.Email).Result;
            if (gotUser is not null)
            {
                BeService.UserManager.AddToRoleAsync(gotUser, role).Wait();
            }

            BeService.DbContext.SaveChanges();
        }
    }
}
    