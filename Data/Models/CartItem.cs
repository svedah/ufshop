namespace ufshop.Data.Models;

public class CartItem
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required int Price { get; set; }
    public required int Amount { get; set; }
    public required bool Uploadable { get; set; }
    public required virtual ShopItem ShopItem { get; set; }
    public required virtual HashSet<CartFile> Uploads { get; set; }
    // public required virtual HashSet<CartItemProperty> Properties { get; set; }
}