using Microsoft.AspNetCore.Mvc.ModelBinding;
using ufshop.Data.Models;
namespace ufshop.Services;

public class ManagePageService
{
    public readonly BeService beService;
    public readonly Shop Shop;

    public ManagePageService(BeService srv)
    {
        beService = srv;
        new ShopService(srv).GetShop(beService.DomainPrefix, out Shop);
    }
}