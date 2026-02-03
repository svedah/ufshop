using System.Diagnostics.Contracts;
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