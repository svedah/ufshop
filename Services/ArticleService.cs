using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;
using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
namespace ufshop.Services;

public class ArticleService
{
    public readonly BeService beService;
    public ArticleService(BeService srv)
    {
        beService = srv;
    }

    // public bool GetArticle(Guid Id, out ShopItem output)
    // {
    //     //TODO: vad göra om artikel inte finns?
    //     output = new 
    //     return false;
    // }

    public bool GetShopItem(Guid Id, out ShopItem shopItem)
    {
        bool output = false;
        shopItem = null;

        //get shop
        Shop shop;
        ShopService ss = new ShopService(beService);
        if (ss.GetShop(beService.DomainPrefix, out shop))
        {
            if (shop.Items.Where(e => e.Id.Equals(Id)).Any())
            {
                shopItem = shop.Items
                    .Where(e => e.Id.Equals(Id))
                    .Where(e => e.Active)
                    .First();
                output = true;
            }
        }

        return output;
    }

}