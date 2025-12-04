using Microsoft.AspNetCore.Http.Features;
using Microsoft.JSInterop;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Threading.Tasks;
using ufshop.Data.Models;
using ufshop.Services;

public class CustomerInfoService
{
    private readonly BeService beService;
    private readonly IJSRuntime _jsRuntime;
    private readonly string _shopKey;

    public CustomerInfoService(BeService srv)
    {
        beService = srv;
        _jsRuntime = beService.JsRuntime;
        _shopKey = BuildKey();
    }

    private string BuildKey()
    {
        return "customeraddress_" + beService.DomainPrefix;
    }

    public async Task AddOrUpdateAsync()
    {
        // Contract.Assert(item is not null);
        // Contract.Assert(count > 0 && count <= item.ItemsAvailable);
        await SaveAsync();
    }

    public async Task ClearAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", _shopKey);
    }

    private async Task<Dictionary<Guid, int>> LoadAsync()
    {
        Dictionary<Guid, int> output = new Dictionary<Guid, int>();

        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", _shopKey);
        if (!string.IsNullOrEmpty(json))
        {
            output = JsonSerializer.Deserialize<Dictionary<Guid, int>>(json) ?? output;
        }

        return output;        
    }

    private async Task SaveAsync()
    {
        // var json = JsonSerializer.Serialize(input);
        // await _jsRuntime.InvokeVoidAsync("localStorage.setItem", _shopKey, json);
    }

}