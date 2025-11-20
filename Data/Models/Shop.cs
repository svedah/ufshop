namespace ufshop.Data.Models;

public class Shop
{
    public required Guid Id { get; set; }

    // public required virtual ShopOwner Owner { get; set; }
    public required string Prefix { get; set; }//hostname
    public required ApplicationUser Owner{ get; set; }
    public required virtual ShopSetting Settings { get; set; }
    public required virtual HashSet<ShopImage> Images { get; set; }
    public required virtual HashSet<ShopItem> Items { get; set; }
    public required virtual HashSet<ShopOrder> Orders { get; set; }
    public required virtual HashSet<ShopPage> Pages { get; set; }
}