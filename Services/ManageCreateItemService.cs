using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ufshop.Components.UI;
using ufshop.Data.Models;
using ufshop.Helpers;
using ufshop.Shared;

namespace ufshop.Services;

public class ManageCreateItemService
{
    public readonly BeService beService;

    public ManageCreateItemService(BeService srv)
    {
        beService = srv;
    }

    public void AddShopItem(ShopItem input)
    {
        if (input.Id == Guid.Empty)
        {
            input.Id = Guid.NewGuid();
        }
    }

    public bool CanCreateNew(ShopItem newShopItem)
    {
        bool output = false;
        Shop shop;
        ShopService ss = new ShopService(beService);
        if (ss.GetShop(beService.DomainPrefix, out shop))
        {
            bool sameTitleExists = shop.Items.Where(e => e.Title.Equals(newShopItem.Title)).Any();
            bool sameDescriptionExists = shop.Items.Where(e => e.Description.Equals(newShopItem.Description)).Any();;
            bool hasItems = newShopItem.ItemsAvailable > 0;
            bool hasPrice = newShopItem.Price > 0;
            bool hasSortOrder = newShopItem.Order > 0;
            bool hasPrimaryImage = !newShopItem.PrimaryImage.Id.Equals(Guid.Empty);

            output = !sameTitleExists && !sameDescriptionExists && hasItems && hasPrice && hasSortOrder && hasPrimaryImage;
        }
        return output;
    }


    //TODO UNTESTED
    public void CreateNew(ShopItem input)
    {
        Guid Id = Guid.NewGuid();
        ShopItem newShopItem = new ShopItem
        {
            Id = Id,
            Title = input.Title,
            Description = input.Description,
            ItemsAvailable = input.ItemsAvailable,
            Price = input.Price,
            Order = input.Order,
            Active = input.Active,
            PrimaryImage = input.PrimaryImage,
            Images = input.Images,
            Properties = input.Properties
        };

        Shop shop;
        ShopService ss = new ShopService(beService);
        ss.GetShop(beService.DomainPrefix, out shop);

        beService.DbContext.ShopItems.Add(newShopItem);

        shop.Items.Add(newShopItem);
        beService.DbContext.Shops.Update(shop);

        beService.DbContext.SaveChanges();
    }


}