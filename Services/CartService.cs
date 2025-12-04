using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Threading.Tasks;
using ufshop.Data.Models;
using ufshop.Services;

public class CartService
{
    private readonly BeService beService;
    private readonly IJSRuntime _jsRuntime;
    private readonly string _shopKey;

    public CartService(BeService srv)
    {
        beService = srv;
        _jsRuntime = beService.JsRuntime;
        _shopKey = BuildShopKey();
    }

    private string BuildShopKey()
    {
        return "cart_" + beService.DomainPrefix;
    }

    public async Task<KeyValuePair<ShopItem, int>> GetCartContentItem(Guid Id)
    {
        var cart = await LoadCartAsync();
        ShopItem si = beService.DbContext.ShopItems.Where(e => e.Id.Equals(Id)).FirstOrDefault()!;
        int amount = -1;

        foreach(KeyValuePair<Guid, int> kvp in cart)
        {
            if (kvp.Key.Equals(Id))
            {
                // if (beService.DbContext.ShopItems.Where(e => e.Id.Equals(kvp.Key)).Any())
                // {
                amount = kvp.Value;
                // }
            }
        }
        return new KeyValuePair<ShopItem, int>(si, amount);
    }

    public async Task<List<KeyValuePair<ShopItem, int>>> GetCartContent()
    {
        var output = new List<KeyValuePair<ShopItem, int>>();

        var cart = await LoadCartAsync();
        foreach(KeyValuePair<Guid, int> kvp in cart)
        {
            if (beService.DbContext.ShopItems.Where(e => e.Id.Equals(kvp.Key)).Any())
            {
                ShopItem si = beService.DbContext.ShopItems
                                .Where(e => e.Id.Equals(kvp.Key))
                                .Include(e => e.PrimaryImage)
                                .Include(e => e.Images)
                                .First();
                output.Add(new KeyValuePair<ShopItem, int>(si, kvp.Value));
            }
        }

        return output;
    }

    public async Task AddOrUpdateAsync(ShopItem item, int count)
    {
        Contract.Assert(item is not null);
        Contract.Assert(count > 0 && count <= item.ItemsAvailable);

        var cartDict = await LoadCartAsync();

        if (cartDict.ContainsKey(item.Id))
        {
            cartDict[item.Id] = count;
        }
        else
        {
            cartDict.Add(item.Id, count);
        }

        await SaveCartAsync(cartDict);
    }

    public async Task ClearAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", _shopKey);
    }

    public async Task DeleteAsync(ShopItem item)
    {
        var cartDict = await LoadCartAsync();

        cartDict.Remove(item.Id);

        await SaveCartAsync(cartDict);
    }

    private async Task<Dictionary<Guid, int>> LoadCartAsync()
    {
        Dictionary<Guid, int> output = new Dictionary<Guid, int>();

        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", _shopKey);
        if (!string.IsNullOrEmpty(json))
        {
            output = JsonSerializer.Deserialize<Dictionary<Guid, int>>(json) ?? output;
        }

        return output;        
    }

    private async Task SaveCartAsync(Dictionary<Guid, int> input)
    {
        var json = JsonSerializer.Serialize(input);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", _shopKey, json);
    }

    //getcartasync
    // public async Task<HashSet<CartItem>> GetCartAsync()
    // {
    //     HashSet<CartItem> output = new HashSet<CartItem>();

    //     var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", _shopKey);
    //     if (!string.IsNullOrEmpty(json))
    //     {
    //         output = JsonSerializer.Deserialize<HashSet<CartItem>>(json) ?? output;
    //     }

    //     return output;
    // }

    //savecartasync
    // public async Task SaveCartAsync(HashSet<CartItem> input)
    // {
    //     var json = JsonSerializer.Serialize(input);
    //     await _jsRuntime.InvokeVoidAsync("localStorage.setItem", _shopKey, json);
    // }
    
    //clearcartasync
    // public async Task ClearCartAsync()
    // {
    //     await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", _shopKey);
    // }

    // public bool CartItemExists(HashSet<CartItem> cart, CartItem input, out CartItem item)
    // {
    //     bool output = false;
    //     item = null;
    //     foreach(CartItem cartItem in cart)
    //     {
    //         if (cartItem.ShopItem.Id.Equals(input.ShopItem.Id));
    //         {
    //             item = cartItem;
    //             output = true;
    //             break;
    //         }
    //     }
    //     return output;
    // }

    //addasync
    // public async Task AddOrUpdateAsync(CartItem input)
    // {
    //     var cart = await GetCartAsync();

    //     //remove old item
    //     CartItem oldItem;
    //     if (CartItemExists(cart, input, out oldItem))
    //     {
    //         await RemoveAsync(oldItem);
    //         cart = await GetCartAsync();
    //     }

    //     //new item
    //     cart.Add(input);
    //     await SaveCartAsync(cart);
    // }

    //removeasync
    // public async Task RemoveAsync(CartItem input)
    // {
    //     var cart = await GetCartAsync();
    //     cart.Remove(input);
    //     await SaveCartAsync(cart);
    // }

}