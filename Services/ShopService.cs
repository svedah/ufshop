using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;
using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
namespace ufshop.Services;

public class ShopService
{
    public readonly BeService beService;
    public ShopService(BeService srv)
    {
        beService = srv;
    }

    public bool ShopExists(string domainPrefix)
    {
        Contract.Assert(domainPrefix is not null);
        Contract.Assert(domainPrefix.Length > 2);
        return beService.DbContext.Shops.Where(e => e.Prefix.Equals(domainPrefix)).Any();
    }

    public bool GetShop(string domainPrefix, out Shop shop)
    {
        Contract.Assert(domainPrefix is not null);
        Contract.Assert(domainPrefix.Length > 2);
        if (!beService.DbContext.Shops.Where(e => e.Prefix.Equals(domainPrefix)).Any())
        {
            shop = null!; //(Shop)default;
            return false;
        }

        shop = beService.DbContext.Shops
                        .Where(e => e.Prefix.Equals(domainPrefix))

                        .Include(e => e.Settings)
                        .Include(e => e.Settings.ContactInfo)
                        .Include(e => e.Settings.ContactInfo.SocialMedia)
                        .Include(e => e.Settings.LogoImage)

                        .Include(e => e.Images)

                        .Include(e => e.Items)
                        .Include(e => e.Items).ThenInclude(e => e.Images)
                        .Include(e => e.Items).ThenInclude(e => e.PrimaryImage)
                        .Include(e => e.Items).ThenInclude(e => e.Properties)
                        .Include(e => e.Items).ThenInclude(e => e.Properties).ThenInclude(e => e.Options)

                        .Include(e => e.Orders)
                        .Include(e => e.Orders).ThenInclude(e => e.Customer)
                        .Include(e => e.Orders).ThenInclude(e => e.Customer).ThenInclude(e => e.Cart)
                        .Include(e => e.Orders).ThenInclude(e => e.Customer).ThenInclude(e => e.Cart).ThenInclude(e => e.CartItems)
                        .Include(e => e.Orders).ThenInclude(e => e.Customer).ThenInclude(e => e.Cart).ThenInclude(e => e.CartItems).ThenInclude(e => e.ShopItem)
                        .Include(e => e.Orders).ThenInclude(e => e.Customer).ThenInclude(e => e.Cart).ThenInclude(e => e.Properties)
                        .Include(e => e.Orders).ThenInclude(e => e.Customer).ThenInclude(e => e.Cart).ThenInclude(e => e.Properties).ThenInclude(e => e.Image)
                        .Include(e => e.Orders).ThenInclude(e => e.Customer).ThenInclude(e => e.Cart).ThenInclude(e => e.Properties).ThenInclude(e => e.Options)
                        





                        .Include(e => e.Orders).ThenInclude(e => e.Customer).ThenInclude(e => e.CustomerInfo)


                        .Include(e => e.Pages)
                        .Include(e => e.Pages).ThenInclude(e => e.ShopPageFragments)
                        .Include(e => e.Pages).ThenInclude(e => e.ShopPageFragments).ThenInclude(e => e.Image)


                        .First();
        // Shop = beService.DbContext.Shops
        //         .Where(e => e.Prefix.Equals(domainPrefix)
        //         .First()
        //         .Include(e=>e.Pages)

        return true;
    }
}