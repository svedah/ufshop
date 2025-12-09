using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using ufshop.Components.Account.Pages.Manage;
using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
namespace ufshop.Services;

public class ShopOrderService
{
    public readonly BeService beService;
    public ShopOrderService(BeService srv)
    {
        beService = srv;
    }

    public void Save(ShopOrder input)
    {
        bool exists = beService.DbContext.ShopOrders.Where(e => e.Id.Equals(input.Id)).Any();

        if (exists)
        {
            beService.DbContext.ShopOrders.Update(input);
        }
        else
        {
            beService.DbContext.ShopOrders.Add(input);
        }
        
        beService.DbContext.SaveChanges();
    }
}