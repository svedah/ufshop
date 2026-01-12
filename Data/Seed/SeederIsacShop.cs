using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ufshop.Data.Models;
using ufshop.Services;


namespace ufshop.Data.Seed;
public class SeederIsacShop
{
    readonly DbContext DbContext;
    public SeederIsacShop(DbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void Seed(ApplicationUser user)
    {
        var logoimage = MockShopImage();
        var socialmedia = MockShopSocialMedia();
        var contactinfo = MockShopContactInfo(socialmedia);
        var settings = MockShopSetting(contactinfo, logoimage);
        var pages = MockShopPages();

        var shop = new Shop
        {
            Id = Guid.NewGuid(),
            Active = true,
            Paid = true,
            Prefix = "isac",
            Owner = user,
            Settings = settings,
            Images = new HashSet<ShopImage>(),
            Items = new HashSet<ShopItem>(),
            Orders = new HashSet<ShopOrder>(),
            Pages = pages //new HashSet<ShopPage>()
        };

        DbContext.Set<Shop>().Add(shop);
        DbContext.SaveChanges();
    }

    private ShopImage MockShopImage()
    {
        return new ShopImage
        {
            Id = Guid.NewGuid(),
            Created = DateTime.Now,
            Filename = "IsacShop.jpeg",
            AltText = "Isacshoppens Logo"
        };
    }

    private ShopSocialMedia MockShopSocialMedia()
    {
        return new ShopSocialMedia
        {
            Facebook = "https://www.facebook.com/isac",
            Instagram = "https://instagram.com/isac",
            LinkedIn = "https://linkedin.com/isac",
            TikTok = "https://tiktok.com/isac",
            YouTube = "https://youtube.com/isac"
        };
    }

    private ShopContactInfo MockShopContactInfo(ShopSocialMedia socialmedia)
    {
        return new ShopContactInfo
        {
            Id = Guid.NewGuid(),
            Email = "isac@webbhelp.se",
            MobileNumber = "+46123456789",
            SocialMedia = socialmedia
        };
    }

    private ShopSetting MockShopSetting(ShopContactInfo contactinfo, ShopImage logoimage)
    {
        return new ShopSetting
        {
            Id = Guid.NewGuid(),
            Title = "IsacShop UF",
            SwishNumber = "+461234567890",
            BaseShippingPrice = 100,
            Description = "Isac Shop Beskrivning",
            Layout = "Standard",
            Theme = "Standard",
            ContactInfo = contactinfo,
            LogoImage = logoimage
        };
    }

        private HashSet<ShopPage> MockShopPages()
    {
        var spf1 = new HashSet<ShopPageFragment>
        {
            new ShopPageFragment{
                Id = Guid.NewGuid(),
                Header = "Isac Sida Titel 1",
                Paragraph = "Isac Sida Paragraf 1",
                Order = 1
            },
            new ShopPageFragment{
                Id = Guid.NewGuid(),
                Header = "Isac Sida Titel 2",
                Paragraph = "Isac Sida Paragraf 2",
                Order = 2
            },
            new ShopPageFragment{
                Id = Guid.NewGuid(),
                Header = "Isac Sida Titel 3",
                Paragraph = "Isac Sida Paragraf 3",
                Order = 3
            }
        };

        var pages = new HashSet<ShopPage>
        {
            new ShopPage
            {
                Id = Guid.NewGuid(),
                Header = "Isacs sida",
                Order = 1,
                ShopPageFragments = spf1
            }
        };

        return pages;
    }

}
    