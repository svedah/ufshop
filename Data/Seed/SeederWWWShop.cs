using System.CodeDom;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ufshop.Data.Models;
using ufshop.Services;

namespace ufshop.Data.Seed;
public class SeederWWWShop
{
    readonly DbContext DbContext;
    public SeederWWWShop(DbContext dbContext)
    {
        DbContext = dbContext;
    }

    private ShopImage MockShopImage()
    {
        return new ShopImage
        {
            Id = Guid.NewGuid(),
            Created = DateTime.Now,
            Filename = "WebbHelp_Logo.jpeg",
            AltText = "WebbHelp Logo"
        };
    }

    private ShopSocialMedia MockShopSocialMedia()
    {
        return new ShopSocialMedia
        {
            Facebook = "https://www.facebook.com",
            Instagram = "https://instagram.com",
            LinkedIn = "https://linkedin.com",
            TikTok = "https://tiktok.com",
            YouTube = "https://youtube.com"
        };
    }

    private ShopContactInfo MockShopContactInfo(ShopSocialMedia socialmedia)
    {
        return new ShopContactInfo
        {
            Id = Guid.NewGuid(),
            Email = "info@webbhelp.se",
            MobileNumber = "+46777987654",
            SocialMedia = socialmedia
        };
    }

    private ShopSetting MockShopSetting(ShopContactInfo contactinfo, ShopImage logoimage)
    {
        return new ShopSetting
        {
            Id = Guid.NewGuid(),
            Title = "WebbHelp UF",
            SwishNumber = "+46777123456",
            BaseShippingPrice = 0,
            Description = "WebbHelp Beskrivning",
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
                Header = "Header Page 1 Fragment 1",
                Paragraph = "Paragraph Page 1 Fragment 1",
                Order = 1
            },
            new ShopPageFragment{
                Id = Guid.NewGuid(),
                Header = "Header Page 1 Fragment 2",
                Paragraph = "Paragraph Page 1 Fragment 2",
                Order = 2
            },
            new ShopPageFragment{
                Id = Guid.NewGuid(),
                Header = "Header Page 1 Fragment 3",
                Paragraph = "Paragraph Page 1 Fragment 3",
                Order = 3
            }
        };

        var spf2 = new HashSet<ShopPageFragment>
        {
            new ShopPageFragment{
                Id = Guid.NewGuid(),
                Header = "Header Page 2 Fragment 1",
                Paragraph = "Paragraph Page 2 Fragment 1",
                Order = 1
            },
            new ShopPageFragment{
                Id = Guid.NewGuid(),
                Header = "Header Page 2 Fragment 2",
                Paragraph = "Paragraph Page 2 Fragment 2",
                Order = 2
            },
            new ShopPageFragment{
                Id = Guid.NewGuid(),
                Header = "Header Page 2 Fragment 3",
                Paragraph = "Paragraph Page 2 Fragment 3",
                Order = 3
            }
        };

        var spf3 = new HashSet<ShopPageFragment>
        {
            new ShopPageFragment{
                Id = Guid.NewGuid(),
                Header = "Header Page 3 Fragment 1",
                Paragraph = "Paragraph Page 3 Fragment 1",
                Order = 1
            },
            new ShopPageFragment{
                Id = Guid.NewGuid(),
                Header = "Header Page 3 Fragment 2",
                Paragraph = "Paragraph Page 3 Fragment 2",
                Order = 2
            },
            new ShopPageFragment{
                Id = Guid.NewGuid(),
                Header = "Header Page 3 Fragment 3",
                Paragraph = "Paragraph Page 3 Fragment 3",
                Order = 3
            }
        };

        var pages = new HashSet<ShopPage>
        {
            new ShopPage
            {
                Id = Guid.NewGuid(),
                Header = "Page 1 Header",
                Order = 1,
                ShopPageFragments = spf1
            },
            new ShopPage
            {
                Id = Guid.NewGuid(),
                Header = "Page 2 Header",
                Order = 2,
                ShopPageFragments = spf2

            },
            new ShopPage
            {
                Id = Guid.NewGuid(),
                Header = "Page 3 Header",
                Order = 3,
                ShopPageFragments = spf3
            },

        };

        return pages;
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
            Prefix = "www",
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
}
    