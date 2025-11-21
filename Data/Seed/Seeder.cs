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
        ApplicationUser shopUser;

        SeedRolesAndUsers(out adminUser, out shopUser);

        if (!BeService.DbContext.Set<Shop>().Any())
        {
            if (adminUser is null || shopUser is null)
            {
                throw new Exception("Default users are null after seeding");
            }
            SeedShops(adminUser, shopUser);
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

    void SeedShops(ApplicationUser adminUser, ApplicationUser shopUser)
    {
        if (!BeService.DbContext.Set<Shop>().Any())
        {
            var wwwshop = new SeederWWWShop(BeService.DbContext);
            wwwshop.Seed(adminUser);

            var sampleshop = new SeederSampleShop(BeService.DbContext);
            sampleshop.Seed(shopUser);
        }
    }

    void SeedIsacShop()
    {
        var isacshop = new SeederIsacShop(BeService.DbContext);
        isacshop.Seed();
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

    private void SeedRolesAndUsers(out ApplicationUser adminUser, out ApplicationUser shopUser)//Shop WWW, Shop Isac)
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

            shopUser = new ApplicationUser
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

            SeedUser(adminUser, "lara1234", "Administrator");
            SeedUser(shopUser, "isac1234", "Owner");
        }
        // adminUser = BeService.DbContext.Users.Where(e => e.UserName == "lara@webbhelp.se").First();
        // shopUser = BeService.DbContext.Users.Where(e => e.UserName == "isac@webbhelp.se").First();
        adminUser = BeService.UserManager.FindByEmailAsync("lara@ufshop.nu").Result;
        shopUser = BeService.UserManager.FindByEmailAsync("isac@ufshop.nu").Result;
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
    