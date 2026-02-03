using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ufshop.Data.Models;
using ufshop.Services;
using ufshop.Shared;
using System.Net.Mime;

namespace ufshop.Controllers;

[ApiController]
// [Route("api/[controller]")]
public class SwishRedirectController : ControllerBase
{
    readonly BeService BeService;
    readonly Guid EMPTY = Guid.Empty;

    public SwishRedirectController(BeService beService)
    {
        BeService = beService;
    }


    //TODO: untested
    [HttpGet]
    [Route("[controller]/testpayment/{id?}")]
    // [ProducesResponseType<string>(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult TestPayment(Guid id) //id = shopOrderId
    {
        //TODO: get shop from beservice.domainprefix
        ShopOrder shopOrder = default; //null
        if (GetShopOrderFromShop(id, ref shopOrder) && shopOrder is not null && !shopOrder.Id.Equals(EMPTY))
        {
            //if there is:
            //get shoporder, set Status to ShopOrderStatus.SwishTriggered
            //OK
            shopOrder.Status = ShopOrderStatus.SwishTriggered;
            BeService.DbContext.ShopOrders.Update(shopOrder);
            BeService.DbContext.SaveChanges();

            //calc cart total
            int cartTotal = CalculateCartTotal(id);
            //get customer phone number
            string customerPhoneNumber = GetCustomerPhoneNumber(id);
            //extract 8 first characters from guid
            string guidPart = ExtractGuidPart(id);
            //build message "ufshop%20abcdabcd" where abcdabcd are from guid
            string message = BuildMessage(guidPart);

            string redirectUrl = BuildSwishRedirectUrl(customerPhoneNumber, cartTotal, message);

            // return Content(redirectUrl, "text/plain");
            
            return new RedirectResult(redirectUrl);

        }
        else
        {
            //if not, return not found
            return new NotFoundResult();
            
        }
    }



    //TODO: untested
    [HttpGet]
    [Route("[controller]/payment/{id?}")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Payment(Guid id) //id = shopOrderId
    {
        //TODO: get shop from beservice.domainprefix
        ShopOrder shopOrder = default; //null
        if (GetShopOrderFromShop(id, ref shopOrder) && shopOrder is not null && !shopOrder.Id.Equals(EMPTY))
        {
            //if there is:
            //get shoporder, set Status to ShopOrderStatus.SwishTriggered
            //OK
            shopOrder.Status = ShopOrderStatus.SwishTriggered;
            BeService.DbContext.ShopOrders.Update(shopOrder);
            BeService.DbContext.SaveChanges();

            //calc cart total
            int cartTotal = CalculateCartTotal(id);
            //get customer phone number
            string customerPhoneNumber = GetCustomerPhoneNumber(id);
            //extract 8 first characters from guid
            string guidPart = ExtractGuidPart(id);
            //build message "ufshop%20abcdabcd" where abcdabcd are from guid
            string message = BuildMessage(guidPart);

            string redirectUrl = BuildSwishRedirectUrl(customerPhoneNumber, cartTotal, message);

            return new RedirectResult(redirectUrl);

        }
        else
        {
            //if not, return not found
            return new NotFoundResult();
            
        }
    }

    private string BuildSwishRedirectUrl(string phoneNumber, int amount, string message)
    {
        //"https://app.swish.nu/1/p/sw/?sw=0700123456&amt=100&cur=SEK&msg=ett%20litet%20test&src=qr";
        string uriEncodedMessage = System.Web.HttpUtility.UrlEncode(message);
        string output = "https://app.swish.nu/1/p/sw/?sw=" +
                        phoneNumber + 
                        "&amt=" +
                        amount.ToString() +
                        "&cur=SEK" +
                        "&msg=" +
                        uriEncodedMessage +
                        "&src=qr";
        return output;
    }

    private bool GetShopOrderFromShop(Guid id, ref ShopOrder shopOrder)
    {
        bool output = false;
        var shopService = new ShopService(BeService);
        Shop shop;
        if (!id.Equals(EMPTY) && shopService.GetShop(BeService.DomainPrefix, out shop))
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
        ShopOrderService sos = new ShopOrderService(BeService);
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
            ShopService ss = new ShopService(BeService);
            Shop shop;
            if (ss.GetShop(BeService.DomainPrefix, out shop))
            {
                output += shop.Settings.BaseShippingPrice;
            }
        }

        return output;
    }


    private string GetCustomerPhoneNumber(Guid shopOrderId)
    {
        string output = string.Empty;
        ShopOrderService sos = new ShopOrderService(BeService);
        if (sos.Exists(shopOrderId))
        {
            ShopOrder shopOrder = sos.Get(shopOrderId);
            CustomerInfo customerInfo = shopOrder.CustomerInfo;
            output = customerInfo.Phone;
        }
        
        return output;
    }

    private string ExtractGuidPart(Guid shopOrderId)
    {
        string output = string.Empty;
        if (!shopOrderId.Equals(EMPTY))
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
        return BeService.DomainPrefix + "." + Constants.DOMAINNAME + ":" + identifier.ToLower();
    }
}