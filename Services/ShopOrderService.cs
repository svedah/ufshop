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

    public ShopOrder Empty()
    {
        //Om det här någonsin körs avsäger jag mig allt ansvar...
        return new ShopOrder
        {
            Id = Guid.Empty,
            Cart = new CartService(beService).Empty(),
            Created = DateTime.UnixEpoch,
            CustomerInfo = new CustomerInfoService(beService).Empty(),
            Status = ShopOrderStatus.Rejected  
        };
    }


    public bool Exists(Guid shopOrderId)
    {
        // bool output = beService.DbContext.ShopOrders.Where(e => e.Id.Equals(shopOrderId)).AsNoTracking().Any();
        bool output = false;
        var ss = new ShopService(beService);
        Shop shop;
        if (ss.GetShop(beService.DomainPrefix, out shop))
        {
            output = shop.Orders.Where(e => e.Id.Equals(shopOrderId)).Any();
        }
        return output;
    }

    public ShopOrder Get(Guid id)
    {
        ShopOrder shopOrder = beService.DbContext.ShopOrders
                        .Where(e => e.Id.Equals(id))

                        .Include(e => e.Cart)
                        .ThenInclude(e => e.CartItems)
                        .ThenInclude(e => e.ShopItem)
                        .ThenInclude(e => e.PrimaryImage)

                        .Include(e => e.CustomerInfo)
                        
                        .First();
        return shopOrder;
    }

    // public string GetGeus()
    // {
    //     Shop shop;
    //     ShopService shopService = new ShopService(beService);
    //     if (shopService.GetShop(beService.DomainPrefix, out shop))
    //     {
    //         //build hashset of all geuses in shop.
    //         HashSet<string> Geuses = new HashSet<string>();
    //         foreach(ShopOrder order in shop.Orders)
    //         {
    //             if(order.Geus.Length == Shared.Constants.GEUSLENGTH)
    //             {
    //                 Geuses.Add(order.Geus);
    //             }
    //         }

    //         //Generate random Geus until unique found
    //         string newGeus = string.Empty;
    //         do
    //         {
    //             newGeus = Helpers.Geus.Generate(Shared.Constants.GEUSLENGTH);
    //         } 
    //         while (Geuses.Contains(newGeus));

    //         return newGeus;
    //     }
    //     return string.Empty;
    // }

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