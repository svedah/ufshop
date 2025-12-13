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

public class ManageOrdersService
{
    public readonly BeService beService;

    public ManageOrdersService(BeService srv)
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
}
