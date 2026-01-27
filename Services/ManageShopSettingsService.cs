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

    public List<string> GetThemes()
    {
        List<string> output = new List<string>();

        string themeRoot = beService.wwwroot + "css" + Path.DirectorySeparatorChar;
        DirectoryInfo di = new DirectoryInfo(themeRoot);
        FileInfo[] files = di.GetFiles("*.min.css");

        foreach(FileInfo file in files)
        {
            string filename = file.Name.Replace(".min.css", string.Empty);
            output.Add(filename);
        }

        output.Sort();

        return output;
    }

    public string GetShopTheme()
    {
        string output = string.Empty;

        var availableThemes = GetThemes();
        var currentShopTheme = Shop.Settings.Theme;

        if (!availableThemes.Contains(currentShopTheme))
        {
            output = "bootstrap";    
        }
        else
        {
            output = currentShopTheme;
        }

        return output;
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
