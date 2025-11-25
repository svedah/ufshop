using Microsoft.AspNetCore.Mvc.ModelBinding;
using ufshop.Data.Models;
namespace ufshop.Services;

public class ManageShopSettingsService
{
    public readonly BeService beService;
    public Shop Shop;

    public ManageShopSettingsService(BeService srv)
    {
        beService = srv;
        new ShopService(srv).GetShop(beService.DomainPrefix, out Shop);
    }

    public void SaveShop()
    {
        //1. update & save shopsocialmedia shop.settings.contactinfo.socialmedia
        beService.DbContext.ShopSocialMedias.Update(Shop.Settings.ContactInfo.SocialMedia);
        //2. update & save shopcontactinfo shop.settings.contactinfo
        beService.DbContext.ShopContactInfos.Update(Shop.Settings.ContactInfo);
        //3. update & save shopsetting shop.settings
        beService.DbContext.ShopSettings.Update(Shop.Settings);
        //4. update & save shop shop
        beService.DbContext.Shops.Update(Shop);
        //5. save db
        beService.DbContext.SaveChanges();
    }
}
