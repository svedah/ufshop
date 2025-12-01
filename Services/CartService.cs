using Microsoft.JSInterop;
using System.Text.Json;
using ufshop.Data.Models;
using ufshop.Services;

public class CartService
{
    private readonly BeService beService;
    private readonly IJSRuntime _jsRuntime;
    private readonly string _shopKey;

    // public CartService(IJSRuntime jsRuntime, BeService srv)
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

    //getcartasync
    public async Task<HashSet<CartItem>> GetCartAsync()
    {
        HashSet<CartItem> output = new HashSet<CartItem>();

        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", _shopKey);
        if (!string.IsNullOrEmpty(json))
        {
            output = JsonSerializer.Deserialize<HashSet<CartItem>>(json) ?? output;
        }

        return output;
    }

    //savecartasync
    public async Task SaveCartAsync(HashSet<CartItem> input)
    {
        var json = JsonSerializer.Serialize(input);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", _shopKey, json);
    }
    
    //clearcartasync
    public async Task ClearCartAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", _shopKey);
    }

    public bool CartItemExists(HashSet<CartItem> cart, CartItem input, out CartItem item)
    {
        bool output = false;
        item = null;
        foreach(CartItem cartItem in cart)
        {
            if (cartItem.ShopItem.Id.Equals(input.ShopItem.Id));
            {
                item = cartItem;
                output = true;
                break;
            }
        }
        return output;
    }

    //addasync
    public async Task AddOrUpdateAsync(CartItem input)
    {
        var cart = await GetCartAsync();

        //remove old item
        CartItem oldItem;
        if (CartItemExists(cart, input, out oldItem))
        {
            await RemoveAsync(oldItem);
            cart = await GetCartAsync();
        }

        //new item
        cart.Add(input);
        await SaveCartAsync(cart);
    }

    //removeasync
    public async Task RemoveAsync(CartItem input)
    {
        var cart = await GetCartAsync();
        cart.Remove(input);
        await SaveCartAsync(cart);
    }

}