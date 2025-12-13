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

    public async Task<bool> MakeOrderAsync(List<CartItem> cartItems, CustomerInfo customerInfo)
    {
        //spara alla cartItems
        // CartItemService cartItemService = new CartItemService(beService);
        // await cartItemService.SaveAsync(cartItems);

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


        //skapa och spara shopOrder
        ShopOrder shopOrder = new ShopOrder
        {
            Id = Guid.NewGuid(),
            Created = DateTime.Now,
            Cart = cart,
            CustomerInfo = customerInfo,
            Status = ShopOrderStatus.Unpaid,
            
        };
        ShopOrderService shopOrderService = new ShopOrderService(beService);
        await shopOrderService.SaveAsync(shopOrder);

        //TODO: add shoporder to shop
        ShopService shopService = new ShopService(beService);
        shopService.Add(shopOrder);



        return true;

    }


}