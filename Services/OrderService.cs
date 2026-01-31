using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using ufshop.Components.Account.Pages.Manage;
using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
namespace ufshop.Services;

public class OrderService
{
    public readonly BeService beService;
    public OrderService(BeService srv)
    {
        beService = srv;
    }

    public async Task<Guid> MakeOrderAsync(List<CartItem> cartItems, CustomerInfo customerInfo)
    {
        //skapa och spara cart
        Cart cart = new Cart
        {
            Id = Guid.NewGuid(),
            CartItems = cartItems.ToHashSet()
        };
        CartService cartService = new CartService(beService);
        await cartService.SaveAsync(cart);


        //spara customer
        CustomerInfoService customerInfoService = new CustomerInfoService(beService);
        //every order might have unique info's but same addressee
        //force new id, dont reuse customerinfo for many orders
        customerInfo.Id = Guid.NewGuid();
        await customerInfoService.SaveDBAsync(customerInfo);

        ShopOrderService shopOrderService = new ShopOrderService(beService);

        //generera unikt guid för shopOrder
        Guid shopOrderId = Guid.NewGuid();

        //skapa och spara shopOrder
        ShopOrder shopOrder = new ShopOrder
        {
            Id = shopOrderId,
            Created = DateTime.Now,
            Cart = cart,
            CustomerInfo = customerInfo,
            Status = ShopOrderStatus.Unpaid,
        };
        await shopOrderService.SaveAsync(shopOrder);

        //add shoporder to shop
        ShopService shopService = new ShopService(beService);
        shopService.Add(shopOrder);

        return shopOrderId;
    }


}