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

    public bool MakeOrder(List<CartItem> cartItems, CustomerInfo customerInfo)
    {
        //spara alla cartItems
        CartItemService cartItemService = new CartItemService(beService);
        cartItemService.Save(cartItems);

        Cart cart = new Cart
        {
            Id = Guid.NewGuid(),
            CartItems = cartItems.ToHashSet()
        };

        //spara cart
        CartService cartService = new CartService(beService);
        cartService.Save(cart);

        //spara customer
        CustomerInfoService customerInfoService = new CustomerInfoService(beService);
        customerInfoService.Save(customerInfo);

        ShopOrder shopOrder = new ShopOrder
        {
            Id = Guid.NewGuid(),
            Created = DateTime.Now,
            Cart = cart,
            CustomerInfo = customerInfo,
            Status = ShopOrderStatus.Unpaid,
        };

        //spara shopOrder
        ShopOrderService shopOrderService = new ShopOrderService(beService);
        shopOrderService.Save(shopOrder);

        return true;






        
        return false;        
    }


}