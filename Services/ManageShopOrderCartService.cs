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

public class ManageShopOrderCartService
{
    public readonly BeService beService;

    public ManageShopOrderCartService(BeService srv)
    {
        beService = srv;
    }

    public int BaseShippingPrice()
    {
        ShopService shopService = new ShopService(beService);
        Shop shop;
        if (shopService.GetShop(beService.DomainPrefix, out shop))
        {
            return shop.Settings.BaseShippingPrice;
        }
        return 0;
    }

    public int TotalSum(Cart cart)
    {
        int output = 0;
        foreach(CartItem cartItem in cart.CartItems)
        {
            output += cartItem.Amount * cartItem.Price;
        }
        output += BaseShippingPrice();
        return output;
    }
}