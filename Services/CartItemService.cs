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

    public async Task SaveAsync(CartItem item)
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
        await beService.DbContext.SaveChangesAsync();
    }

    public void Save(List<CartItem> items)
    {
        foreach(CartItem item in items)
        {
            Save(item);
        }
    }

    public async Task SaveAsync(List<CartItem> items)
    {


        foreach(CartItem item in items)
        {
            //fetch item.ShopItem from DB to avoid tracking issues
            Guid shopItemId = item.ShopItem.Id;
            item.ShopItem = beService.DbContext.ShopItems.Where(e => e.Id.Equals(shopItemId)).First();

            // await SaveAsync(item);
            // bool exists = beService.DbContext.CartItems.Where(e => e.Id.Equals(item.Id)).Any();

            //BUG: reuse of cartitems might happen if same user places two separate orders
            //create new id
            item.Id = Guid.NewGuid();

            // if (!exists)
            // {
            try
            {
                await beService.DbContext.CartItems.AddAsync(item);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            // }
            // else
            // {
            //     beService.DbContext.CartItems.Update(item);
            // }
        }
        await beService.DbContext.SaveChangesAsync();
    }


}
//jojes branch fungerar 3.0