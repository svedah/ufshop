using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using ufshop.Components.Account.Pages.Manage;
using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
namespace ufshop.Services;

public class ShopOrderStatusService
{
    public readonly BeService beService;
    public ShopOrderStatusService(BeService srv)
    {
        beService = srv;
    }

    public ShopOrderStatus SetStatus(Guid Id, int newStatus)
    {
        bool exists = beService.DbContext.ShopOrders.Where(e => e.Id.Equals(Id)).Any();

        if (exists)
        {
            ShopOrder shopOrder = beService.DbContext.ShopOrders.Where(e => e.Id.Equals(Id)).First();

            ShopOrderStatus newShopOrderStatus;
            switch(newStatus)
            {
                case 0:
                    newShopOrderStatus = ShopOrderStatus.Unpaid;
                break;
                case 1:
                    newShopOrderStatus = ShopOrderStatus.Paid;
                break;
                case 2:
                    newShopOrderStatus = ShopOrderStatus.Shipped;
                break;
                case 3:
                    newShopOrderStatus = ShopOrderStatus.Rejected;
                break;
                default:
                    throw new Exception("undefined shoporderstatus");
                break;
            }

            shopOrder.Status = newShopOrderStatus;
            beService.DbContext.ShopOrders.Update(shopOrder);
            beService.DbContext.SaveChanges();
            return newShopOrderStatus;
        }
        
        //BUG: kommer vi hit fanns inte ordern.
        return ShopOrderStatus.Unpaid;
    }

    public ShopOrder ReclaimToStock(Guid Id)
    {
        ShopOrder output = new ShopOrderService(beService).Empty();

        //återför shoporders "inventarier" till shoppens lager
        //sätter sedan shoporder till cancelled -> "makulerad"
        bool exists = beService.DbContext.ShopOrders.Where(e => e.Id.Equals(Id)).Any();

        if (exists)
        {
            output = beService.DbContext.ShopOrders
                                        .Where(e => e.Id.Equals(Id))
                                        .Include(e => e.Cart)
                                        .ThenInclude(e => e.CartItems)
                                        .ThenInclude(e => e.ShopItem)
                                        .Include(e => e.CustomerInfo)
                                        .First();

            //reclaims to stock but does not change cart
            foreach(CartItem ci in output.Cart.CartItems)
            {
                //TODO: reclaim to stock here
                ShopItem si = beService.DbContext.ShopItems.Where(e => e.Id.Equals(ci.ShopItem.Id)).First();
                si.ItemsAvailable += ci.Amount;
                beService.DbContext.ShopItems.Update(si);
            }

            //cancel order
            output.Status = ShopOrderStatus.Cancelled;
            //spara i databasen
            beService.DbContext.ShopOrders.Update(output);
            beService.DbContext.SaveChanges();
        }

        return output;
    }

}