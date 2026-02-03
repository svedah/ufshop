using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
namespace ufshop.Services;

public class OrderConfirmationService
{
    public readonly BeService beService;
    public OrderConfirmationService(BeService srv)
    {
        beService = srv;
    }

    public bool ShopOrderExists(Guid shopOrderId)
    {
        ShopOrderService sos = new ShopOrderService(beService);
        bool output = sos.Exists(shopOrderId);
        return output;
    }

    public ShopOrder GetShopOrder(Guid shopOrderId)
    {
        ShopOrder output;
        if (ShopOrderExists(shopOrderId))
        {
            ShopOrderService sos = new ShopOrderService(beService);
            //TODO: check if complete object
            output = sos.Get(shopOrderId);
        }
        else
        {
            output = new ShopOrderService(beService).Empty();
        }
        return output;
    }

    public bool GetShop(out Shop shop)
    {
        ShopService ss = new ShopService(beService);
        return ss.GetShop(beService.DomainPrefix, out shop);
    }

}