using System.Diagnostics.Contracts;
using System.Reflection.Metadata;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using ufshop.Components.UI;
using ufshop.Data.Models;
using ufshop.Helpers;
using ufshop.Shared;

namespace ufshop.Services;

public class ManageShopOrdersService
{
    public readonly BeService beService;

    public ManageShopOrdersService(BeService srv)
    {
        beService = srv;
    }

    public List<ShopOrder> GetAllShopOrders()
    {
        var tmp = beService.DbContext.Shops
                    .Where(e => e.Prefix.Equals(beService.DomainPrefix))
                    .Include(e => e.Orders)
                    .Include(e => e.Orders).ThenInclude(e => e.Cart)
                    .Include(e => e.Orders).ThenInclude(e => e.Cart).ThenInclude(e => e.CartItems)
                    .Include(e => e.Orders).ThenInclude(e => e.CustomerInfo)
                    .First();

        List<ShopOrder> orders = tmp.Orders.OrderBy(e=>e.Created).ToList();
        return orders;
    }

    public IEnumerable<Tuple<string,string>> GetAllShopOrdersDataList()
    {
        var shop = beService.DbContext.Shops
                    .Where(e => e.Prefix.Equals(beService.DomainPrefix))
                    .Include(e => e.Orders)
                    .First();

        foreach(var order in shop.Orders)
        {
            string fullGuid = order.Id.ToString();
            string shortGuid = fullGuid.Split('-')[0];
            yield return new Tuple<string, string>(fullGuid, shortGuid);
        }
        
        yield break;
    }

    public ShopOrder GetShopOrder(Guid id)
    {
        ShopOrder output = new ShopOrderService(beService).Empty();
        output.Id = Guid.Empty;

        var shop = beService.DbContext.Shops
            .Where(e => e.Prefix.Equals(beService.DomainPrefix))
            .Include(e => e.Orders)
            .First();
        
        if (shop.Orders.Where(e => e.Id.Equals(id)).Any())
        {
            output = shop.Orders.Where(e => e.Id.Equals(id)).First();
        }
        return output;
    }

    public List<ShopOrder> GetFilteredShopOrders(int filter)
    {
        Contract.Assert(filter >= -1 && filter <= 4);

        List<ShopOrder> output = new List<ShopOrder>();
        var list = GetAllShopOrders();
        switch(filter)
        {
            case 0: //unpaid
                output = list.Where(e => e.Status.Equals(ufshop.Data.Models.ShopOrderStatus.Unpaid)).ToList();
                break;
            case 1: //paid
                output = list.Where(e => e.Status.Equals(ufshop.Data.Models.ShopOrderStatus.Paid)).ToList();
                break;
            case 2://shipped
                output = list.Where(e => e.Status.Equals(ufshop.Data.Models.ShopOrderStatus.Shipped)).ToList();
                break;
            case 3://rejected
                output = list.Where(e => e.Status.Equals(ufshop.Data.Models.ShopOrderStatus.Rejected)).ToList();
                break;
            case 4://swishtriggered
                output = list.Where(e => e.Status.Equals(ufshop.Data.Models.ShopOrderStatus.SwishTriggered)).ToList();
                break;
            default: //-1 - All
                output = list.ToList();
                break;
        }

        return output;
    }
}
