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

    public IEnumerable<ShopOrder> GetAllShopOrders()
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

    public IEnumerable<ShopOrder> GetFilteredShopOrders(int filter)
    {
        Contract.Assert(filter >= 0 && filter <= 4);

        List<ShopOrder> output = new List<ShopOrder>();
        var list = GetAllShopOrders();
        switch(filter)
        {
            case 0:
                output = list.Where(e => e.Status.Equals(ufshop.Data.Models.ShopOrderStatus.Unpaid)).ToList();
                break;
            case 1:
                output = list.Where(e => e.Status.Equals(ufshop.Data.Models.ShopOrderStatus.Paid)).ToList();
                break;
            case 2:
                output = list.Where(e => e.Status.Equals(ufshop.Data.Models.ShopOrderStatus.Shipped)).ToList();
                break;
            case 3:
                output = list.Where(e => e.Status.Equals(ufshop.Data.Models.ShopOrderStatus.Rejected)).ToList();
                break;
            case 4:
                output = list.ToList();
                break;
            default:
                output = list.ToList();
                break;
            
        }
        return output;
    }
}
