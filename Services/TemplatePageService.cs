using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
namespace ufshop.Services;

public class TemplatePageService
{
    public readonly BeService beService;
    public readonly string DomainPrefix;
    public readonly Shop Shop;
    
    public TemplatePageService(BeService srv)
    {
        beService = srv;
        DomainPrefix = beService.DomainPrefix;

        if (new ShopService(beService).GetShop(DomainPrefix, out Shop))
        {
            // get shop succeeded
        }
        else
        {
            // get shop fail
        }

    }
}