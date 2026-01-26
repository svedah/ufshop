namespace ufshop.Data.Models;

public class ShopItem
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required int ItemsAvailable { get; set; } //0=outofstock
    public required int Price { get; set; }
    public required string Description { get; set; }
    public required int Order { get; set; }
    public required bool Active { get; set; }//published on site or not
    // public required bool Deleted { get; set; }//shopitems can never be deleted once a cart contains it
    // public required virtual Shop Shop { get; set; }
    public required bool Uploadable { get; set; }
    public required bool Rabatt { get; set; }//rabattnotis på varan
    public required virtual ShopImage PrimaryImage { get; set; }
    public required virtual HashSet<ShopImage> Images { get; set; }

    // public required virtual HashSet<ShopItemProperty> Properties { get; set; }
}

