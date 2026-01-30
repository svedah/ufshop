using System.Diagnostics.Contracts;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ufshop.Components.Account.Pages.Manage;
using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
namespace ufshop.Services;

public class ShopOrderService
{
    public readonly BeService beService;
    public ShopOrderService(BeService srv)
    {
        beService = srv;
    }

    public string GetGeus()
    {
        Shop shop;
        ShopService shopService = new ShopService(beService);
        if (shopService.GetShop(beService.DomainPrefix, out shop))
        {
            //build hashset of all geuses in shop.
            HashSet<string> Geuses = new HashSet<string>();
            foreach(ShopOrder order in shop.Orders)
            {
                if(order.Geus.Length == Shared.Constants.GEUSLENGTH)
                {
                    Geuses.Add(order.Geus);
                }
            }

            //Generate random Geus until unique found
            string newGeus = string.Empty;
            do
            {
                newGeus = Helpers.Geus.Generate(Shared.Constants.GEUSLENGTH);
            } 
            while (Geuses.Contains(newGeus));

            return newGeus;
        }
        return string.Empty;
    }

    public void Save(ShopOrder input)
    {
        bool exists = beService.DbContext.ShopOrders.Where(e => e.Id.Equals(input.Id)).Any();

        if (exists)
        {
            beService.DbContext.ShopOrders.Update(input);
        }
        else
        {
            beService.DbContext.ShopOrders.Add(input);
        }
        
        beService.DbContext.SaveChanges();
    }

    public async Task SaveAsync(ShopOrder input)
    {
        bool exists = beService.DbContext.ShopOrders.Where(e => e.Id.Equals(input.Id)).Any();

        if (exists)
        {
            beService.DbContext.ShopOrders.Update(input);
        }
        else
        {
            beService.DbContext.ShopOrders.Add(input);
        }
        
        await beService.DbContext.SaveChangesAsync();
    }

}