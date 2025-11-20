using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
namespace ufshop.Services;

public class ImagesSelectorService
{
    public readonly BeService beService;
    public readonly ShopService shopService;
    public readonly Shop shop;
    
    public ImagesSelectorService(BeService srv)
    {
        beService = srv;

        string DomainPrefix = DomainHelper.ExtractSubDomain(beService.HttpContextAccessor);
        shopService = new ShopService(srv);
        shopService.GetShop(beService.DomainPrefix, out shop);
    }

    public HashSet<ShopImage> GetAllImages()
    {
        return shop.Images;
    }

}