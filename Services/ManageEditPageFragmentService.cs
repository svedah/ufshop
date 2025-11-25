using ufshop.Components.UI;
using ufshop.Data.Models;
using ufshop.Helpers;
using ufshop.Shared;

namespace ufshop.Services;

public class ManageEditPageFragmentService
{
    public readonly BeService beService;

    public ManageEditPageFragmentService(BeService srv)
    {
        beService = srv;
    }

    public HashSet<ShopPageFragment> AllShopPageFragments()
    {
        HashSet<ShopPageFragment> output = new HashSet<ShopPageFragment>();

        Shop shop;
        ShopService ss = new ShopService(beService);
        ss.GetShop(beService.DomainPrefix, out shop);

        foreach(ShopPage page in shop.Pages)
        {
            foreach(ShopPageFragment shopPageFragment in page.ShopPageFragments)
            {
                output.Add(shopPageFragment);
            }
        }

        return output;
    }

    public bool GetShopPageFragment(Guid Id, out ShopPageFragment fragment)
    {
        bool output = false;

        // HashSet<ShopPageFragment> ShopPageFragments = AllShopPageFragments();

        // output = ShopPageFragments.Where(e => e.Id.Equals(Id)).Any();
        output = beService.DbContext.ShopPageFragments.Where(e=>e.Id.Equals(Id)).Any();

        // fragment = output ? ShopPageFragments.Where(e => e.Id.Equals(Id)).First() : default!;
        if (output)
        {
            fragment = beService.DbContext.ShopPageFragments.Where(e => e.Id.Equals(Id)).First();
        }
        else
        {
            fragment = default!;
        }

        return output;
    }

    public ShopPageFragment MockEmptyShopPageFragment()
    {
        return new ShopPageFragment
        {
            Id = Guid.Empty,
            Header = string.Empty,
            Paragraph = string.Empty,
            Image = null,
            Order = 0
        };
    }
}

