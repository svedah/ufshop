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

public class ManageShopOrderService
{
    public readonly BeService beService;

    public ManageShopOrderService(BeService srv)
    {
        beService = srv;
    }

    public ShopOrder GetShopOrder(Guid Id)
    {
        ShopOrder output;
        bool exists = beService.DbContext.ShopOrders.Where(e => e.Id.Equals(Id)).Any();

        if (exists)
        {
            output = beService.DbContext.ShopOrders.Where(e => e.Id.Equals(Id)).First();
        }
        else
        {
            output = Empty();
        }

        return output;
    }

    private ShopOrder Empty()
    {
        return new ShopOrder
        {
            Id = Guid.Empty,
            Created = DateTime.UnixEpoch,
            Cart = new CartService(beService).Empty(),
            CustomerInfo = new CustomerInfoService(beService).Empty(),
            Status = ShopOrderStatus.Rejected
        };
    }
}
