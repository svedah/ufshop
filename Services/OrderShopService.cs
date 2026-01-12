using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using ufshop.Components.Account.Pages.Manage;
using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
namespace ufshop.Services;

public class OrderShopService
{
    public readonly BeService beService;
    public OrderShopService(BeService srv)
    {
        beService = srv;
    }

    public bool IsFormDataValid(string prefix, string company, string email, string phone)
    {
        bool validPrefix = ValidatePrefix(prefix);
        bool validCompany = company.Length > 4;
        bool validEmail =  new EmailHelper().IsValidEmail(email);
        bool validPhone = phone.Length >= 10;

        return validPrefix && validCompany && validEmail && validPhone;
    }

    public bool IsValidPrefix(string input)
    {
        return ValidatePrefix(input);
    }

    private bool ValidatePrefix(string input)
    {
        bool alreadyExists = beService.DbContext.Shops.Where(e => e.Prefix.Equals(input)).Any();
        bool alreadyOrdered = beService.DbContext.UFShopOrders.Where(e => e.Prefix.Equals(input)).Any();
        bool isValidPrefix = Regex.IsMatch(input, @"^[a-z]{3,16}$");
        return !alreadyExists && !alreadyOrdered && isValidPrefix;
    }

    public bool MakeOrder(string prefix, string company, string email, string phone, string companytype, bool assisted)
    {
        UFShopOrder newUfShopOrder = new UFShopOrder
        {
            Id = Guid.NewGuid(),
            Prefix = prefix,
            Email = email,
            Title = company,
            Assisted = assisted,
            UF = companytype.Equals("uf"),
            Active = false,
            Paid = false,
            Created = DateTime.Now
        };
        return false;
    }


}