using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using System.Diagnostics.Contracts;

using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
using ufshop.Shared;

namespace ufshop.Services;

public class AdministrationCreateShopService
{
    public readonly BeService beService;
    public AdministrationCreateShopService(BeService srv)
    {
        beService = srv;
    }

    public IQueryable<Shop> AllShops()
    {
        return beService.DbContext.Shops.AsQueryable();
    }

    public IQueryable<UFShopOrder> AllUfShopOrders()
    {
        return beService.DbContext.UFShopOrders.AsQueryable();
    }

    public UFShopOrder EmptyUfShopOrder()
    {
        return new UFShopOrder{
            Id = Guid.Empty,
            Prefix = "empty",
            Email = "em@il.com",
            Phone = "0000000000",
            Title = "Untitled",
            UF = true,
            Assisted = false,
            Active = false,
            Paid = false,
            Created = DateTime.UnixEpoch
        };
    }

    public bool IsValid(UFShopOrder input)
    {
        bool output = false;
        bool notnull = input is not null;

        ShopService shopService = new ShopService(beService);

        if (notnull && input != null)
        {
            bool isOrder = input is not null;
            bool validGuid = !input.Id.Equals(Guid.Empty) &&
                             !shopService.ShopExistsById(input.Id);

            bool validPrefix =  !string.IsNullOrWhiteSpace(input.Prefix) && 
                                input.Prefix.Length >= 3 && 
                                !input.Prefix.Equals("empty") &&
                                !shopService.ShopExistsByDomainPrefix(input.Prefix);

            bool validEmail = new EmailHelper().IsValidEmail(input.Email);

            bool validPhone = input.Phone.Trim().Length >= 10;

            bool validTitle =   input.Title.Trim().Length >= 3 &&
                                !shopService.ShopExistsByTitle(input.Title);

            bool validCreationDate = input.Created > DateTime.UnixEpoch;

            output = isOrder && validGuid && validPrefix && validEmail && validPhone && validTitle && validCreationDate;
        }

        return notnull && output;
    }

    public bool CreateNewShop(UFShopOrder ufShopOrder, string ufPassword)
    {
        bool output = false;
        
        bool validShop = IsValid(ufShopOrder);
        if (validShop)
        {
            ApplicationUser user;
            bool userExists = CreateOrGetUser(ufShopOrder.Email, ufShopOrder.Phone, ufPassword, out user);
            if (userExists)
            {
                Contract.Assert(user is not null);
                
                ShopImage shopImage = beService.DbContext.ShopImages.Where(e => e.Id.Equals(Constants.EMPTYIMAGEGUID)).First();

                ShopSocialMedia newShopSocialMedia = new ShopSocialMedia
                {
                    Id = Guid.NewGuid(),
                    Facebook = "https://www.facebook.com/",
                    Instagram = "https://instagram.com/",
                    LinkedIn = "https://linkedin.com/",
                    TikTok = "https://tiktok.com/",
                    YouTube = "https://youtube.com/"
                };

                ShopContactInfo newShopContactInfo = new ShopContactInfo {
                    Id = Guid.NewGuid(),
                    Email = ufShopOrder.Email,
                    MobileNumber = ufShopOrder.Phone,
                    SocialMedia = newShopSocialMedia
                };

                ShopSetting newShopSetting = new ShopSetting
                {
                    Id = Guid.NewGuid(),
                    Title = ufShopOrder.Title,
                    SwishNumber = ufShopOrder.Phone,
                    BaseShippingPrice = 100,
                    Description = "Beskrivning " + ufShopOrder.Title,
                    Layout = "Standard",
                    Theme = "bootstrap",
                    LogoImage = shopImage,
                    ContactInfo = newShopContactInfo

                };
                Shop newShop = new Shop
                {
                    Id = ufShopOrder.Id,
                    Active = true,
                    Paid = true,
                    Prefix = ufShopOrder.Prefix,
                    Owner = user,
                    Settings = newShopSetting,
                    Images = new HashSet<ShopImage>(),
                    Items = new HashSet<ShopItem>(),
                    Orders = new HashSet<ShopOrder>(),
                    Pages = new HashSet<ShopPage>(),
                };
                ShopService shopService = new ShopService(beService);
                shopService.Add(newShop);
                output = true;
            }
        }

        return output;
    }

    private bool CreateOrGetUser(string email, string phone, string password, out ApplicationUser user)
    {
        bool output = false;

        bool userExists = beService.DbContext.Users.Any(e => e.UserName == email);

        if (userExists)
        {
            user = (ApplicationUser)beService.DbContext.Users.Where(e => e.UserName == email).First();
            output = true;
        }
        else
        {
            user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = email.ToLower(),
                Email = email.ToLower(),
                NormalizedEmail = email.ToUpper(),
                NormalizedUserName = email.ToUpper(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false
            };

            if (CreateUser(user, password, "Owner"))
            {
                output = true;
            }
            else
            {
                //ajajaj...
                throw new NotImplementedException("could not create user");
            }
        }
        return output;
    }

    private bool CreateUser(ApplicationUser user, string password, string role)
    {
        Contract.Assert(user is not null);
        Contract.Assert(user.Email is not null && user.Email.Length >= 6);//TODO: emailhelper
        Contract.Assert(password is not null && password.Length >= 8);//TODO: Constants.MINIMUMPASSWORDLENGTH?

        bool userExists = beService.DbContext.Users.Any(e => e.UserName == user.UserName);
        if (!userExists)
        {
            user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, password);

            var userStore = new UserStore<ApplicationUser>(beService.DbContext);
            userStore.CreateAsync(user).Wait();

            //role
            var gotUser = beService.UserManager.FindByEmailAsync(user.Email).Result;
            if (gotUser is not null)
            {
                beService.UserManager.AddToRoleAsync(gotUser, role).Wait();
            }

            beService.DbContext.SaveChanges();
            userExists = beService.DbContext.Users.Any(e => e.UserName == user.UserName);
        }
        return userExists;
    }

    public void DeleteUfShopOrder(Guid Id)
    {
        bool exists = beService.DbContext.UFShopOrders.Where(e=>e.Id.Equals(Id)).Any();
        if (exists)
        {
            var ufso = beService.DbContext.UFShopOrders.Where(e=>e.Id.Equals(Id)).First();
            beService.DbContext.UFShopOrders.Remove(ufso);
            beService.DbContext.SaveChanges();
        }
    }

    public bool GetUFShopOrder(Guid SelectedId, out UFShopOrder ufShopOrder)
    {
        bool output = false;

        bool ufShopOrderExists = beService.DbContext.UFShopOrders.Where(e => e.Id.Equals(SelectedId)).Any();

        if (ufShopOrderExists)
        {
            ufShopOrder = beService.DbContext.UFShopOrders.Where(e => e.Id.Equals(SelectedId)).First();
            output = true;
        }
        else
        {
            ufShopOrder = EmptyUfShopOrder();
        }

        return output;
    }

    public void MarkPaidAndActive(UFShopOrder input)
    {
        bool exists = beService.DbContext.UFShopOrders.Where(e=>e.Id.Equals(input.Id)).Any();
        if (exists)
        {
            UFShopOrder ufso = beService.DbContext.UFShopOrders.Where(e=>e.Id.Equals(input.Id)).First();
            ufso.Active = true;
            ufso.Paid = true;
            beService.DbContext.UFShopOrders.Update(ufso);
            beService.DbContext.SaveChanges();
        }
    }



    // public bool ShopExists(string domainPrefix)
    // {
    //     Contract.Assert(domainPrefix is not null);
    //     Contract.Assert(domainPrefix.Length > 2);
    //     return beService.DbContext.Shops.Where(e => e.Prefix.Equals(domainPrefix)).Any();
    // }

    // public bool GetShop(string domainPrefix, out Shop shop)
    // {
    //     Contract.Assert(domainPrefix is not null);
    //     Contract.Assert(domainPrefix.Length > 2);
    //     if (!beService.DbContext.Shops.Where(e => e.Prefix.Equals(domainPrefix)).Any())
    //     {
    //         shop = null!; //(Shop)default;
    //         return false;
    //     }

    //     shop = beService.DbContext.Shops
    //                     .Where(e => e.Prefix.Equals(domainPrefix))

    //                     .Include(e => e.Owner)

    //                     .Include(e => e.Settings)
    //                     .Include(e => e.Settings.ContactInfo)
    //                     .Include(e => e.Settings.ContactInfo.SocialMedia)
    //                     .Include(e => e.Settings.LogoImage)

    //                     .Include(e => e.Images)

    //                     .Include(e => e.Items)
    //                     .Include(e => e.Items).ThenInclude(e => e.Images)
    //                     .Include(e => e.Items).ThenInclude(e => e.PrimaryImage)
    //                     // .Include(e => e.Items).ThenInclude(e => e.Properties)
    //                     // .Include(e => e.Items).ThenInclude(e => e.Properties).ThenInclude(e => e.Options)

    //                     .Include(e => e.Orders)
    //                     .Include(e => e.Orders).ThenInclude(e => e.Cart)
    //                     .Include(e => e.Orders).ThenInclude(e => e.Cart).ThenInclude(e => e.CartItems)
    //                     .Include(e => e.Orders).ThenInclude(e => e.CustomerInfo)
    //                     // .Include(e => e.Orders).ThenInclude(e => e.Status)

    //                     .Include(e => e.Pages)
    //                     .Include(e => e.Pages).ThenInclude(e => e.ShopPageFragments)
    //                     .Include(e => e.Pages).ThenInclude(e => e.ShopPageFragments).ThenInclude(e => e.Image)

    //                     .First();
    //     // Shop = beService.DbContext.Shops
    //     //         .Where(e => e.Prefix.Equals(domainPrefix)
    //     //         .First()
    //     //         .Include(e=>e.Pages)

    //     return true;
    // }

    // public bool GetShopByOwner(ApplicationUser user, out Shop shop)
    // {
    //     Contract.Assert(user is not null);

    //     if (beService.DbContext.Shops.Where(e => e.Owner == user).Any())
    //     {
    //         shop = beService.DbContext.Shops.Where(e => e.Owner == user).First();
    //         return true;
    //     }

    //     shop = null!;
    //     return false;
    // }

    // public void Add(ShopOrder input)
    // {
    //     //TODO
    //     Shop shop;
    //     if (GetShop(beService.DomainPrefix, out shop))
    //     {
    //         shop.Orders.Add(input);
    //         beService.DbContext.Shops.Update(shop);
    //         beService.DbContext.SaveChanges();
    //     } 
    // }
}