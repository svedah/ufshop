using ufshop.Data;
using ufshop.Data.Models;
using ufshop.Helpers;
namespace ufshop.Services;

public class ImageSelectorService
{
    public readonly BeService beService;
    public readonly ShopService shopService;
    public readonly Shop shop;
    
    public ImageSelectorService(BeService srv)
    {
        beService = srv;

        shopService = new ShopService(srv);
        shopService.GetShop(beService.DomainPrefix, out shop);
    }

    public HashSet<ShopImage> GetAllImages()
    {
        return shop.Images;
    }

    public HashSet<ShopImage> GetImages(ShopItem shopItem)
    {
        HashSet<ShopImage> output;

        if (shop.Items.Where(e => e.Id.Equals(shopItem.Id)).Any())
        {
            output = shop.Items.Where(e => e.Id.Equals(shopItem.Id)).First().Images;
        }
        else
        {
            output = new HashSet<ShopImage>();
        }

        return output;
    }

    public bool GetImage(Guid Id, ref ShopImage image)
    {
        bool output = shop.Images.Where(e => e.Id.Equals(Id)).Any();

        if (output)
        {
            image = shop.Images.Where(e => e.Id.Equals(Id)).First();
        }

        return output;
    }
}