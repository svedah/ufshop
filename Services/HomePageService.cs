using Microsoft.AspNetCore.Mvc.ModelBinding;
using ufshop.Data.Models;
namespace ufshop.Services;

public class HomePageService
{
    public readonly BeService beService;
    public readonly Shop Shop;

    public string LogoImageFile
    {
        get
        {
            return "img/" + Shop.Settings.LogoImage.Filename;
        }
    }

    public string LogoImageAltText
    {
        get
        {
            return Shop.Settings.LogoImage.AltText;
        }
    }

    public string Title
    {
        get
        {
            return Shop.Settings.Title;
        }
    }

    public string Description
    {
        get
        {
            return Shop.Settings.Description;
        }
    }

    public IQueryable<ShopPage> Pages
    {
        get
        {
            if (Shop is not null && Shop.Pages is not null)
            {
                return Shop.Pages.OrderBy(e => e.Order).AsQueryable();
            }
            else
            {
                return Enumerable.Empty<ShopPage>().AsQueryable();
            }
        }
    }
    public IQueryable<ShopItem> ShopItems
    {
        get
        {
            if (Shop is not null && Shop.Items is not null)
            {
                return Shop.Items.OrderBy(e => e.Order).AsQueryable();
            }
            else
            {
                return Enumerable.Empty<ShopItem>().AsQueryable();
            }
        }
    }
    public HomePageService(BeService srv)
    {
        beService = srv;
        new ShopService(srv).GetShop(beService.DomainPrefix, out Shop);
    }
}