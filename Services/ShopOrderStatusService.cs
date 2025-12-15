using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using ufshop.Components.Account.Pages.Manage;
using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
namespace ufshop.Services;

public class ShopOrderStatusService
{
    public readonly BeService beService;
    public ShopOrderStatusService(BeService srv)
    {
        beService = srv;
    }

}