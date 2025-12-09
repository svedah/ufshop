// using System.Diagnostics.Contracts;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration.UserSecrets;
// using ufshop.Data;
// using ufshop.Data.Models;
// using ufshop.Helpers;
// using ufshop.Shared;
// namespace ufshop.Services;

// public class ShopItemPropertyService
// {
//     public readonly BeService beService;
//     public ShopItemPropertyService(BeService srv)
//     {
//         beService = srv;
//     }

//     public ShopItemProperty Empty()
//     {
//         var emptyImage = beService.DbContext.ShopImages.Where(e => e.Id.Equals(Constants.EMPTYIMAGEGUID)).First();

//         return new ShopItemProperty
//         {
//             Id = Guid.NewGuid(),
//             Name = "Egenskapsnamn",
//             Uploadable = false,
//             Options = new HashSet<ShopItemPropertyOption>(),
//             Image = emptyImage
//         };
//     }

//     public bool GetShopItemProperty(Guid Id, out ShopItemProperty sip)
//     {
//         bool result = beService.DbContext.ShopItemProperties.Where(e => e.Id.Equals(Id)).Any();
//         if (result)
//         {
//             sip = beService.DbContext.ShopItemProperties.Where(e => e.Id.Equals(Id)).First();
//         }
//         else
//         {
//             sip = Empty();
//         }
//         return result;
//     }
// }