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

    public bool IsFormDataValid(string prefix, string company, string email, string phone, bool agreement)
    {
        bool validPrefix = ValidatePrefix(prefix);
        bool validCompany = company.Length > 4;
        bool validEmail =  new EmailHelper().IsValidEmail(email);
        bool validPhone = new String(phone.Where(Char.IsDigit).ToArray()).Length >= 10;

        return validPrefix && validCompany && validEmail && validPhone && agreement;
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

    public Guid MakeOrder(string prefix, string company, string email, string phone, bool companytype, bool assisted, bool agreement)
    {
        Guid output = Guid.Empty;

        if (IsFormDataValid(prefix, company, email, phone, agreement))
        {
            Guid newId = Guid.NewGuid();
            UFShopOrder newUfShopOrder = new UFShopOrder
            {
                Id = newId,
                Prefix = prefix,
                Email = email,
                Phone = phone,
                Title = company,
                Assisted = assisted,
                UF = companytype,
                Active = false,
                Paid = false,
                Created = DateTime.Now
            };

            beService.DbContext.UFShopOrders.Add(newUfShopOrder);
            beService.DbContext.SaveChanges();

            output = newId;
        }

        return output;
    }


}