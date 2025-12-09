using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using SQLitePCL;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Threading.Tasks;
using ufshop.Data.Models;
using ufshop.Services;

public class CartItemService
{
    private readonly BeService beService;

    public CartItemService(BeService srv)
    {
        beService = srv;
    }

    public void Save(CartItem item)
    {
        
        bool exists = beService.DbContext.CartItems.Where(e => e.Id.Equals(item.Id)).Any();

        if (!exists)
        {
            beService.DbContext.CartItems.Add(item);
        }
        else
        {
            beService.DbContext.CartItems.Update(item);
        }
        beService.DbContext.SaveChanges();
    }

    public void Save(List<CartItem> items)
    {
        foreach(CartItem item in items)
        {
            Save(item);
        }
    }


}