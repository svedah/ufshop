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
    private readonly string _localStorageKey;

    public CustomerInfoService(BeService srv)
    {
        beService = srv;
        _jsRuntime = beService.JsRuntime;
        _localStorageKey = BuildKey();
    }

    private string BuildKey()
    {
        return "customerinfo_" + beService.DomainPrefix;
    }

    // public async Task AddOrUpdateAsync()
    // {
    //     // Contract.Assert(item is not null);
    //     // Contract.Assert(count > 0 && count <= item.ItemsAvailable);
    //     await SaveAsync();
    // }

    public async Task ClearAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", _localStorageKey);
    }

    public async Task<CustomerInfo> LoadAsync()
    {
        CustomerInfo output;

        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", _localStorageKey);
        if (!string.IsNullOrEmpty(json))
        {
            output = JsonSerializer.Deserialize<CustomerInfo>(json) ?? Empty();
        }
        else
        {
            output = Empty();
        }

        return output;        
    }

    public CustomerInfo RemoveWhiteSpace(CustomerInfo customerInfo)
    {
        return new CustomerInfo
        {
            Id = customerInfo.Id,
            FirstName = customerInfo.FirstName.Trim(),
            LastName = customerInfo.LastName.Trim(),
            StreetName = customerInfo.StreetName.Trim(),
            StreetNo = customerInfo.StreetNo.Trim(),
            ZipCode = customerInfo.ZipCode.Trim(),
            City = customerInfo.City.Trim(),
            Email = customerInfo.Email.Trim(),
            Phone = customerInfo.Phone.Trim(),
            Info = customerInfo.Info.Trim()
        };
    }

    public async Task SaveAsync(CustomerInfo input)
    {
        var json = JsonSerializer.Serialize(input);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", _localStorageKey, json);
    }

    public CustomerInfo Empty()
    {
        return new CustomerInfo
        {
            Id = Guid.NewGuid(),
            FirstName = string.Empty,
            LastName = string.Empty,
            StreetName = string.Empty,
            StreetNo = string.Empty,
            ZipCode = string.Empty,
            City = string.Empty,
            Email = string.Empty,
            Phone = string.Empty,
            Info = string.Empty
        };
    }

    public bool Valid(CustomerInfo info)
    {
        return  ValidFirstName(info.FirstName) &&
                ValidLastName(info.LastName) &&
                ValidStreetName(info.StreetName) &&
                ValidZipCode(info.ZipCode) &&
                ValidCity(info.City) &&
                ValidEmailPhone(info.Email, info.Phone) &&
                ValidInfo(info.Info);
    }

    public bool ValidFirstName(string input)
    {
        return input is not null && input.Length >= 2 && !input.Any(char.IsDigit);
    }

    public bool ValidLastName(string input)
    {
        return input is not null && input.Length >= 2 && !input.Any(char.IsDigit);
    }
    public bool ValidStreetName(string input)
    {
        return input is not null && input.Length >= 3 && input.Any(char.IsSeparator) && input.Any(char.IsDigit);
    }

    public bool ValidZipCode(string input)
    {
        bool output = false;
        if (input is not null)
        {
            string truncated = input.Replace(" ", "");
            bool isDigits = truncated.All(char.IsDigit);
            bool hasValidLength = truncated.Length >= 5;
            output = isDigits && hasValidLength;
        }

        return output;

    }

    public bool ValidCity(string input)
    {
        return input is not null && input.Length >= 2 && !input.Any(char.IsDigit);
    }

    public bool ValidEmailPhone(string email, string phone)
    {
        return ValidEmail(email) || ValidPhone(phone);
    }

    private bool ValidEmail(string input)
    {
        var eh = new ufshop.Helpers.EmailHelper();
        return input is not null && eh.IsValidEmail(input);
    }

    private bool ValidPhone(string input)
    {
        bool output = false;
        if (input is not null)
        {
            string truncated = input.Replace(" ", "").Replace("-", "").Replace("+", "");
            bool isDigits = truncated.All(char.IsDigit);
            bool hasValidLength = truncated.Length >= 9;

            output = isDigits && hasValidLength;
        }

        return output;
    }

    public bool ValidInfo(string input)
    {
        return true;
    }

    public void Save(CustomerInfo customerInfo)
    {
        bool exists = beService.DbContext.CustomerInfos.Where(e => e.Id.Equals(customerInfo.Id)).Any();

        if (exists)
        {
            beService.DbContext.CustomerInfos.Update(customerInfo);
        }
        else
        {
            beService.DbContext.CustomerInfos.Update(customerInfo);
        }
        beService.DbContext.SaveChanges();
    }




}