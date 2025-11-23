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

    public ShopPageFragment AddNewShopPageFragment(int fragments)
    {
        return new ShopPageFragment
        {
            Id = Guid.NewGuid(),
            Header = string.Empty,
            Paragraph = string.Empty,
            Order = fragments,
            Image = null
        };
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
        //shoppagefragments first
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

        //page last
        bool pageExists = beService.DbContext.ShopPages.Where(e => e.Id.Equals(shopPage)).Any();
        if (pageExists)
        {
            beService.DbContext.ShopPages.Update(shopPage);
        }
        else
        {
            beService.DbContext.ShopPages.Add(shopPage);
        }

        beService.DbContext.SaveChanges();
    }

}