using SQLitePCL;
using ufshop.Components.UI;
using ufshop.Data.Models;
using ufshop.Helpers;
using ufshop.Shared;

namespace ufshop.Services;

public class ManageEditPageService
{
    public readonly BeService beService;

    public ManageEditPageService(BeService srv)
    {
        beService = srv;
    }

    public HashSet<ShopPage> AllShopPages()
    {
        Shop shop;
        ShopService ss = new ShopService(beService);
        ss.GetShop(beService.DomainPrefix, out shop);
        return shop.Pages;
    }

    public bool GetShopPage(Guid Id, out ShopPage page)
    {
        bool output = false;
        if (beService.DbContext.ShopPages.Where(e=>e.Id.Equals(Id)).Any())
        {
            page = beService.DbContext.ShopPages.Where(e=>e.Id.Equals(Id)).First();
            output = true;
        }
        else
        {
            page = default!;
        }
        return output;
    }

    public ShopPage MockEmptyShopPage()
    {
        return new ShopPage
        {
            Id = Guid.Empty,
            Header = string.Empty,
            Order = 0,
            ShopPageFragments = new HashSet<ShopPageFragment>()
        };
    }

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

            ShopPageFragment newShopPageFragment = new ShopPageFragment
            {
                Id = Guid.NewGuid(),
                Header = string.Empty,
                Paragraph = string.Empty,
                Order = shopPage.ShopPageFragments.Count,
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

    public void DeleteShopPageFragment(Guid Id)
    {
        if (beService.DbContext.ShopPageFragments.Where(e => e.Id.Equals(Id)).Any())
        {
            ShopPageFragment shopPageFragment = beService.DbContext.ShopPageFragments.Where(e => e.Id.Equals(Id)).First();

            //scan through all shoppages and remove fragment if it exists.
            foreach(ShopPage page in AllShopPages())
            {
                page.ShopPageFragments.RemoveWhere(e => e.Id.Equals(Id));
                beService.DbContext.ShopPages.Update(page);
            }

            //remove shoppagefragment itself
            beService.DbContext.ShopPageFragments.Remove(shopPageFragment);

            //save
            beService.DbContext.SaveChanges();
        }
    }

    public void DeleteShopPage(ShopPage page)
    {
        //delete all shop page fragments
        var spfids = new HashSet<Guid>();
        foreach(ShopPageFragment spf in page.ShopPageFragments)
        {
            spfids.Add(spf.Id);
        }
        // foreach(ShopPageFragment spf in page.ShopPageFragments)
        foreach(Guid id in spfids)
        {
            // beService.DbContext.ShopPageFragments.Remove(spf);
            DeleteShopPageFragment(id);
        }
        //delete page itself
        if (beService.DbContext.ShopPages.Where(e => e.Id.Equals(page.Id)).Any())
        {
            ShopPage dbPage = beService.DbContext.ShopPages.Where(e => e.Id.Equals(page.Id)).First();
            beService.DbContext.ShopPages.Remove(dbPage);
        }
        //save
        beService.DbContext.SaveChanges();
    }

    //TODO: untested
    public void SaveShopPage(ShopPage shopPage)
    {
        //save shoppagefragments
        foreach(ShopPageFragment spf in shopPage.ShopPageFragments)
        {
            bool spfExists = beService.DbContext.ShopPageFragments.Where(e => e.Id.Equals(spf.Id)).Any();
            if (spfExists)
            {
                beService.DbContext.ShopPageFragments.Update(spf);
            }
            else
            {
                beService.DbContext.ShopPageFragments.Add(spf);
            }
        }
        beService.DbContext.SaveChanges();

        //save page
        bool pageExists = beService.DbContext.ShopPages.Where(e => e.Id.Equals(shopPage.Id)).Any();
        if (pageExists)
        {
            beService.DbContext.ShopPages.Update(shopPage);
        }
        else
        {
            beService.DbContext.ShopPages.Add(shopPage);
        }
        beService.DbContext.SaveChanges();

        //Save page to shop
        Shop shop;
        ShopService ss = new ShopService(beService);
        if (ss.GetShop(beService.DomainPrefix, out shop))
        {
            bool shopHasPage = shop.Pages.Where(e => e.Id.Equals(shopPage.Id)).Any();

            if (!shopHasPage)
            {
                shop.Pages.Add(shopPage);
                beService.DbContext.Shops.Update(shop);                
            }
            beService.DbContext.SaveChanges();
        }

    }

}