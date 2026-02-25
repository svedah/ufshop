using System.Diagnostics.Contracts;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
using ufshop.Shared;

namespace ufshop.Services;

public class PaymentService
{
    public readonly BeService beService;
    public PaymentService(BeService srv)
    {
        beService = srv;
    }

    public string BuildSwishDirectUrl(Guid id)
    {
        string output = string.Empty;
        ShopOrder shopOrder = default;//null;
        if (GetShopOrderFromShop(id, ref shopOrder) && shopOrder is not null && !shopOrder.Id.Equals(Guid.Empty))
        {
            int cartTotal = CalculateCartTotal(id);
            string SwishNumber = GetShopSwishNumber();
            string guidPart = ExtractGuidPart(id);
            string message = BuildMessage(guidPart);

            output = BuildSwishRedirectUrl(SwishNumber, cartTotal, message);
        }
        return output;
    }




    private bool GetShopOrderFromShop(Guid id, ref ShopOrder shopOrder)
    {
        bool output = false;
        var shopService = new ShopService(beService);
        Shop shop;
        if (!id.Equals(Guid.Empty) && shopService.GetShop(beService.DomainPrefix, out shop))
        {
            output = shop.Orders.Where(e => e.Id.Equals(id)).Any();
            if (output)
            { 
                shopOrder = shop.Orders.Where(e => e.Id.Equals(id)).First();
            }
        }
        return output;
    }


    private int CalculateCartTotal(Guid shopOrderId)
    {
        int output = 0;
        //get shoporder
        //get cart
        //enumerate shopitems price
        ShopOrderService sos = new ShopOrderService(beService);
        if (sos.Exists(shopOrderId))
        {
            ShopOrder shopOrder = sos.Get(shopOrderId);
            Cart cart = shopOrder.Cart;
            HashSet<CartItem> Items = cart.CartItems;

            foreach(CartItem item in Items)
            {
                output += item.Amount * item.Price; //TODO: rabatt
            }            
            
            //basfraktpris
            ShopService ss = new ShopService(beService);
            Shop shop;
            if (ss.GetShop(beService.DomainPrefix, out shop))
            {
                output += shop.Settings.BaseShippingPrice;
            }
        }

        return output;
    }



    private string ExtractGuidPart(Guid shopOrderId)
    {
        string output = string.Empty;
        if (!shopOrderId.Equals(Guid.Empty))
        {
            string stringShopOrderId = shopOrderId.ToString();
            string[] splitted = stringShopOrderId.Split('-');
            if (splitted.Length > 0)
            {
                output = splitted[0];
            }
        }
        return output;
    }



    private string BuildMessage(string identifier)
    {
        // return BeService.DomainPrefix + "." + Constants.DOMAINNAME + ":" + identifier.ToLower();
        return beService.DomainPrefix + "." + Constants.DOMAINNAME + ":" + identifier.ToLower();
    }


    public string BuildSwishRedirectUrl(string phoneNumber, int amount, string message)
    {
        //"https://app.swish.nu/1/p/sw/?sw=0700123456&amt=100&cur=SEK&msg=ett%20litet%20test&src=qr";
        string uriEncodedMessage = System.Web.HttpUtility.UrlEncode(message);
        string output = "https://app.swish.nu/1/p/sw/?sw=" +
                        phoneNumber.Trim() + 
                        "&amt=" +
                        amount.ToString().Trim() +
                        "&cur=SEK" +
                        "&msg=" +
                        uriEncodedMessage +
                        "&src=qr";
        return output;
    }



    public string GetShopSwishNumber()
    {
        string output = string.Empty;

        Shop shop;
        ShopService ss = new ShopService(beService);
        if (ss.GetShop(beService.DomainPrefix, out shop))
        {
            output = shop.Settings.SwishNumber;
        }

        return output;
    }

    public string BuildOrderConfirmationUrl(Guid id)
    {
        return "/orderconfirmation/" + id.ToString();
    }

    public string BuildSwishRedirectUrl(Guid id)
    {
        string output = string.Empty;
        ShopService ss = new ShopService(beService);
        Shop shop;
        if (ss.GetShop(beService.DomainPrefix, out shop))
        {
            output =    "https://" + 
                        beService.DomainPrefix.ToLower() +
                        "." + 
                        Constants.DOMAINNAME +
                        "/swishredirect/payment/" +
                        id.ToString();
        }
        return output;
    }

    public string BuildQrCodeData(Guid id)
    {
        return System.Web.HttpUtility.UrlEncode(BuildSwishRedirectUrl(id));       
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

}