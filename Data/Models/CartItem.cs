namespace ufshop.Data.Models;

public class CartItem
{
    public required Guid Id { get; set; }
    public required virtual ShopItem ShopItem { get; set; }
    public required int Amount { get; set; }

}