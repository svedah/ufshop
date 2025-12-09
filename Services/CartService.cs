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

    // public async Task<KeyValuePair<ShopItem, int>> GetCartContentItem(Guid Id)
    // {
    //     var cart = await LoadCartAsync();
    //     ShopItem si = beService.DbContext.ShopItems.Where(e => e.Id.Equals(Id)).FirstOrDefault()!;
    //     int amount = -1;

    //     foreach(KeyValuePair<Guid, int> kvp in cart)
    //     {
    //         if (kvp.Key.Equals(Id))
    //         {
    //             amount = kvp.Value;
    //         }
    //     }
    //     return new KeyValuePair<ShopItem, int>(si, amount);
    // }

    public async Task<CartItem> GetCartContentItem(Guid Id)
    {
        CartItem output = null;

        var cart = await LoadCartAsync();
        if (cart.Where(e => e.Id.Equals(Id)).Any())
        {
            output = cart.Where(e => e.Id.Equals(Id)).First();
        }

        return output;
    }


    public async Task<List<CartItem>> GetCartContent()
    {
        var cart = await LoadCartAsync();
        return cart;
    }

    public async Task AddOrUpdateAsync(CartItem input)
    {
        Contract.Assert(input is not null);
        Contract.Assert(input.ShopItem is not null);
        Contract.Assert(input.Amount > 0 && input.Amount <= input.ShopItem.ItemsAvailable);

        var cart = await LoadCartAsync();

        //refresh shopitem
        //TODO: load properties
        ShopItem shopItem = beService.DbContext.ShopItems.Where(e => e.Id.Equals(input.ShopItem.Id)).First();

        bool cartItemExists = cart.Where(e=>e.Id.Equals(input.Id)).Any();

        if (cartItemExists)
        {
            input.ShopItem = shopItem;
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Id.Equals(input.Id))
                {
                    cart[i] = input;
                    break;
                }
            }
        }
        else
        {
            cart.Add(input);
        }


        await SaveCartAsync(cart);
    }

    static public CartItem BuildCartItem(ShopItem input, int amount)//, HashSet<CartItemProperty> cartItemProperties)
    {


        CartItem output = new CartItem
        {
            Id = Guid.NewGuid(),
            Title = input.Title,
            Price = input.Price,
            Amount = amount,
            ShopItem = input,
            Uploadable = input.Uploadable,
            Uploads = new HashSet<CartFile>(),
        };
        return output;
    }

    public async Task ClearAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", _shopKey);
    }

    public async Task DeleteAsync(CartItem item)
    {
        var cart = await LoadCartAsync();
        bool itemExists = cart.Where(e => e.Id.Equals(item.Id)).Any();
        if (itemExists)
        {
            CartItem deleteItem = cart.Where(e => e.Id.Equals(item.Id)).First();
            cart.Remove(deleteItem);
        }
        await SaveCartAsync(cart);
    }


    //TODO: sign cart to protect from tampering
    private async Task<List<CartItem>> LoadCartAsync()
    {
        // Dictionary<Guid, int> output = new Dictionary<Guid, int>();
        List<CartItem> output = new List<CartItem>();

        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", _shopKey);
        //TODO: decrypt
        if (!string.IsNullOrEmpty(json))
        {
            output = JsonSerializer.Deserialize<List<CartItem>>(json) ?? output;
        }

        return output;        
    }

    private async Task SaveCartAsync(List<CartItem> input)
    {
        var json = JsonSerializer.Serialize(input);
        //TODO:encrypt
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", _shopKey, json);
    }

    public void Save(Cart cart)
    {
        CartItemService cartItemService = new CartItemService(beService);
        cartItemService.Save(cart.CartItems.ToList());

        bool exists = beService.DbContext.Carts.Where(e => e.Id.Equals(cart.Id)).Any();

        if (exists)
        {
            beService.DbContext.Carts.Update(cart);
        }
        else
        {
            beService.DbContext.Carts.Add(cart);
        }
        
        beService.DbContext.SaveChanges();
    }

}