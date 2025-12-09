namespace ufshop.Data.Models;

public class Cart
{
    public required Guid Id { get; set; }
    public required virtual HashSet<CartItem> CartItems { get; set; }
    // public required virtual HashSet<ShopItemProperty> Properties { get; set; }

}
