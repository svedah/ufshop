using SQLitePCL;
using ufshop.Components.UI;
using ufshop.Data.Models;
using ufshop.Helpers;
using ufshop.Shared;

namespace ufshop.Services;

public class ManageCreatePageService
{
    public readonly BeService beService;

    public ManageCreatePageService(BeService srv)
    {
        beService = srv;
    }

    //TODO: untested
    public ShopPageFragment AddNewShopPageFragment(ShopPage shopPage)
    {
        ShopPageFragment output = default!;
        Shop shop;
        ShopService ss = new ShopService(beService);
        if (ss.GetShop(beService.DomainPrefix, out shop))
        {
            bool shopPageExists = beService.DbContext.ShopPages.Where(e=>e.Id.Equals(shopPage.Id)).Any();
            if (!shopPageExists)
            {
                beService.DbContext.ShopPages.Add(shopPage);
                beService.DbContext.SaveChanges();
            }

            int count = shopPage.ShopPageFragments.Count + 1;
            ShopPageFragment newShopPageFragment = new ShopPageFragment
            {
                Id = Guid.NewGuid(),
                Header = "Titel sidfragment " + count,
                Paragraph = string.Empty,
                Order = count,
                Image = null
            };

            output = newShopPageFragment;

            shopPage.ShopPageFragments.Add(newShopPageFragment);
            beService.DbContext.ShopPageFragments.Add(newShopPageFragment);
            beService.DbContext.ShopPages.Update(shopPage);
            beService.DbContext.SaveChanges();
        }
        return output;
    }

    public ShopPage MockNewShopPage()
    {
        return new ShopPage
        {
            Id = Guid.NewGuid(),
            Header = string.Empty,
            Order = 0,
            ShopPageFragments = new HashSet<ShopPageFragment>()
        };
    }


    //TODO: untested
    public void SaveShopPage(ShopPage shopPage)
    {
        //save shoppagefragments
        foreach(ShopPageFragment spf in shopPage.ShopPageFragments)
        {
            bool spfExists = beService.DbContext.ShopPageFragments.Where(e=>e.Id.Equals(spf.Id)).Any();

            if (spfExists)
            {
                beService.DbContext.ShopPageFragments.Update(spf);
            }
            else
            {
                beService.DbContext.ShopPageFragments.Add(spf);
            }
        }

        //save shoppage
        bool pageExists = beService.DbContext.ShopPages.Where(e => e.Id.Equals(shopPage.Id)).Any();

        if (pageExists)
        {
            beService.DbContext.ShopPages.Update(shopPage);
        }
        else
        {
            beService.DbContext.ShopPages.Add(shopPage);
        }

        //add shoppage to shop
        Shop shop;
        ShopService ss = new ShopService(beService);
        if (ss.GetShop(beService.DomainPrefix, out shop))
        {
            //BUG: kan, men borde inte, finnas en annan sida med samma guid...
            shop.Pages.RemoveWhere(e=> e.Id.Equals(shopPage.Id));
            shop.Pages.Add(shopPage);
        }

        //save shop
        beService.DbContext.Shops.Update(shop);

        //save database
        beService.DbContext.SaveChanges();
    }
}