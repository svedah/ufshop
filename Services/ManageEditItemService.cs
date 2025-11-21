using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SQLitePCL;
using ufshop.Components.UI;
using ufshop.Data.Models;
using ufshop.Helpers;
using ufshop.Shared;

namespace ufshop.Services;

public class ManageEditItemService
{
    public readonly BeService beService;

    public ManageEditItemService(BeService srv)
    {
        beService = srv;
    }

    public HashSet<ShopItem> AllShopItems()
    {
        Shop shop;
        ShopService ss = new ShopService(beService);
        ss.GetShop(beService.DomainPrefix, out shop);
        return shop.Items;
    }

    public bool GetShopItem(Guid Id, out ShopItem item)
    {
        bool output = false;
        if (beService.DbContext.ShopItems.Where(e=>e.Id.Equals(Id)).Any())
        {
            item = beService.DbContext.ShopItems.Where(e=>e.Id.Equals(Id)).First();
            output = true;
        }
        else
        {
            item = default!;
        }
        return output;
    }

    public bool CanEditItem(ShopItem editShopItem)
    {
        bool output = false;
        Shop shop;
        ShopService ss = new ShopService(beService);
        if (ss.GetShop(beService.DomainPrefix, out shop))
        {
            bool sameTitleExists = shop.Items.Where(e => e.Title.Equals(editShopItem.Title)).Any();
            bool sameDescriptionExists = shop.Items.Where(e => e.Description.Equals(editShopItem.Description)).Any();;
            bool hasItems = editShopItem.ItemsAvailable > 0;
            bool hasPrice = editShopItem.Price > 0;
            bool hasSortOrder = editShopItem.Order > 0;
            bool hasPrimaryImage = !editShopItem.PrimaryImage.Id.Equals(Guid.Empty);

            output = !sameTitleExists && !sameDescriptionExists && hasItems && hasPrice && hasSortOrder && hasPrimaryImage;
        }
        return output;
    }

    public void Edit(ShopItem shopItem)
    {
        bool shopItemIdExists = beService.DbContext.ShopItems.Where(e=>e.Id.Equals(shopItem.Id)).Any();
        bool shopOwnsItem = false;
        bool itemHasUniqueTitleAndDescription = false;

        Shop shop;
        ShopService ss = new ShopService(beService);
        if (shopItemIdExists && ss.GetShop(beService.DomainPrefix, out shop))
        {
            shopOwnsItem = shop.Items.Where(e => e.Id.Equals(shopItem.Id)).Any();

            itemHasUniqueTitleAndDescription = !shop.Items
                .Where(e => e.Id != shopItem.Id)
                .Where(e => e.Title.Equals(shopItem.Title))
                .Where(e => e.Description.Equals(shopItem.Description)).Any();

            if (shopItemIdExists && shopOwnsItem && itemHasUniqueTitleAndDescription)
            {
                beService.DbContext.ShopItems.Update(shopItem);
                beService.DbContext.SaveChanges();
            }
        }

        // TODO: om ufshop väljer att avpublicera måste varan plockas bort från ALLA aktiva carts (jippi...)

    }
}