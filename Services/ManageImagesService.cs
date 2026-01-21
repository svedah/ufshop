using System.Diagnostics.Contracts;
using System.Reflection.Metadata;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ufshop.Components.UI;
using ufshop.Data.Models;
using ufshop.Helpers;
using ufshop.Shared;

namespace ufshop.Services;

public class ManageImagesService
{
    public readonly BeService beService;
    public readonly ShopService shopService;
    private Shop shop;

    public ManageImagesService(BeService srv)
    {
        beService = srv;
        shopService = new ShopService(srv);
        shopService.GetShop(beService.DomainPrefix, out shop);
    }

    public HashSet<ShopImage> GetShopImages()
    {
        shopService.GetShop(beService.DomainPrefix, out shop);
        return shop.Images;
        // HashSet<ShopImage> output = new HashSet<ShopImage>();
        // foreach(ShopImage si in shop.Images)
        // {
        //     output.Add(si);
        // }
        // return output;
    }

    public void UpdateAltText(ShopImage shopImage)
    {
        beService.DbContext.ShopImages.Update(shopImage);
        beService.DbContext.SaveChanges();
        shopService.GetShop(beService.DomainPrefix, out shop);
    }

    //TODO UNTESTED
    public void DeleteImage(ShopImage shopImage)
    {
        Contract.Assert(shopImage is not null);

        if (beService.DbContext.ShopImages.Where(e => e.Id.Equals(shopImage.Id)).Any())
        {
            //vi vet inte vilken entity som använder denna bild så det är bara att börja leta igenom allihopa
            DeleteImageFromShop(shopImage);

            //sedan kan vi sudda från shopimage från databasen
            // DeleteImageFromShopImages(shopImage);

            //och till sist filen självt...
            // string file = beService.wwwroot + shopImage.Filename;
            // if (File.Exists(file))
            // {
            //     File.Delete(file);
            // }
        }
    }

    //TODO UNTESTED
    public void DeleteImageFromShop(ShopImage shopImage)
    {
        DeleteImageFromShopSetting(shopImage);
        DeleteImageFromShopItems(shopImage);
        DeleteImageFromTheShop(shopImage);
        beService.DbContext.SaveChanges();
        shopService.GetShop(beService.DomainPrefix, out shop);
    }

    //TODO UNTESTED
    public void DeleteImageFromShopSetting(ShopImage shopImage)
    {
        if (shop.Settings.LogoImage is not null && shopImage.Id.Equals(shop.Settings.LogoImage.Id))
        {
            //set shop logo to empty
            ShopImage emptyImage = beService.DbContext.ShopImages.Where(e => e.Filename.Equals(Constants.EMPTYIMAGEFILENAME)).First();
            ShopSetting shopSetting = shop.Settings;
            shopSetting.LogoImage = emptyImage;
            beService.DbContext.ShopSettings.Update(shopSetting);
            shopService.GetShop(beService.DomainPrefix, out shop);
        }
    }

    //TODO UNTESTED
    public void DeleteImageFromShopItems(ShopImage shopImage)
    {
        ShopImage emptyImage = beService.DbContext.ShopImages.Where(e=>e.Filename == Constants.EMPTYIMAGEFILENAME).First();

        foreach(ShopItem shopitem in shop.Items)
        {
            bool updateShopItem = false;
            if (shopImage.Id.Equals(shopitem.PrimaryImage.Id))
            {
                shopitem.PrimaryImage = emptyImage;
                updateShopItem = true;
            }

            int removedImages = shopitem.Images.RemoveWhere(e=>e.Id.Equals(shopImage.Id));
            if (updateShopItem || removedImages > 0)
            {
                beService.DbContext.ShopItems.Update(shopitem);
            }
        }
        beService.DbContext.SaveChanges();
    }

    //TODO UNTESTED
    public void DeleteImageFromTheShop(ShopImage shopImage)
    {
        shop.Images.Remove(shopImage);

        if (beService.DbContext.Shops.Where(e=>e.Id.Equals(shop.Id)).Any())
        {
            var cshop = beService.DbContext.Shops.Where(e=>e.Id.Equals(shop.Id)).First();
            cshop.Images.Remove(shopImage);

            beService.DbContext.Shops.Update(cshop);
            beService.DbContext.SaveChanges();
        }
        shopService.GetShop(beService.DomainPrefix, out shop);
    }


    //TODO: UNTESTED
    public void DeleteImageFromShopImages(ShopImage shopImage)
    {//från tabellen

        ShopImage emptyImage = beService.DbContext.ShopImages.Where(e=>e.Id.Equals(Guid.Empty)).First();

        //TODO...
        //1. ta bort från varje sida
        {
            foreach(ShopPage shopPage in shop.Pages)
            {
                foreach(ShopPageFragment spf in shopPage.ShopPageFragments)
                {

                    if (spf.Image is not null && spf.Image.Id.Equals(shopImage.Id))
                    {
                        spf.Image = emptyImage;
                        beService.DbContext.ShopPageFragments.Update(spf);
                    }
                }
            }
        }
        //2. ta bort från varje vara
        {
            foreach(ShopItem shopItem in shop.Items)
            {
                if (shopItem.PrimaryImage.Id.Equals(shopImage.Id))
                {
                    shopItem.PrimaryImage = emptyImage;
                    beService.DbContext.ShopItems.Update(shopItem);
                }
            }
        }
        //3. ta bort från databas
        beService.DbContext.ShopImages.Remove(shopImage);   

        //4. ta bort från wwwroot/img/ och wwwroot/img/t
        if (File.Exists(beService.wwwroot + Constants.IMAGEPATH + shopImage.Filename))
        {
            File.Delete(beService.wwwroot + Constants.IMAGEPATH + shopImage.Filename);
        }
        if (File.Exists(beService.wwwroot + Constants.THUMBIMAGEPATH + shopImage.Filename))
        {
            File.Delete(beService.wwwroot + Constants.THUMBIMAGEPATH + shopImage.Filename);
        }

        //5. spara databasändringar
        beService.DbContext.SaveChanges();

        shopService.GetShop(beService.DomainPrefix, out shop);
    }



    private bool ResizeAndRescaleImage(byte[] imageData, ref byte[] newImageData, ref byte[] newThumbImageData)
    {
        bool output = false;

        SixLabors.ImageSharp.Image image;
        if (ImageSharpHelper.OpenImageFromArray(imageData, out image))
        {
            //crop
            SixLabors.ImageSharp.Image croppedImage = ImageSharpHelper.CropToSquare(image);

            // resize image
            SixLabors.ImageSharp.Image resizedImage = ImageSharpHelper.Resize(croppedImage, Constants.IMAGEWIDTH, Constants.IMAGEHEIGHT);

            // resize thumb image
            SixLabors.ImageSharp.Image resizedThumbImage = ImageSharpHelper.Resize(croppedImage, Constants.THUMBIMAGEWIDTH, Constants.THUMBIMAGEHEIGHT);

            //return image as byte array
            newImageData = ImageSharpHelper.SaveImageToStream(resizedImage, Constants.IMAGEQUALITY).ToArray();

            // return thumb image as byte array
            newThumbImageData = ImageSharpHelper.SaveImageToStream(resizedThumbImage, Constants.THUMBIMAGEQUALITY).ToArray();

            output = true;
        }

        return output;
    }

    public bool AddImage(byte[] imageData)
    {
        Contract.Assert(imageData.Length > 0);

        bool output = false;

        byte[] newImageData = new byte[0];
        byte[] newThumbImageData = new byte[0];
        if (ResizeAndRescaleImage(imageData, ref newImageData, ref newThumbImageData))
        {
            Guid imageId = Guid.NewGuid();
            ShopImage shopImage = new ShopImage
            {
                Id = imageId,
                Created = DateTime.Now,
                Filename = imageId.ToString() + ".jpeg",
                AltText = "Bildbeskrivning"
            };

            //store image
            var storeImagePath = beService.wwwroot + Path.DirectorySeparatorChar + "img" + Path.DirectorySeparatorChar;
            File.WriteAllBytes(storeImagePath + shopImage.Filename, newImageData);

            //store thumb image
            var storeThumbImagePath = beService.wwwroot + Path.DirectorySeparatorChar + "img" + Path.DirectorySeparatorChar + "t" + Path.DirectorySeparatorChar;
            File.WriteAllBytes(storeThumbImagePath + shopImage.Filename, newThumbImageData);

            //store shopimage in db
            beService.DbContext.ShopImages.Add(shopImage);
 
            ShopService shopService = new ShopService(beService);
            Shop shop;
            if (shopService.GetShop(beService.DomainPrefix, out shop))
            {
                shop.Images.Add(shopImage);
                beService.DbContext.Shops.Update(shop);
            }

            beService.DbContext.SaveChanges();
            output = true;
        }

        return output;
    }

}