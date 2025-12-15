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

}