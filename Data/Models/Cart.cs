namespace ufshop.Data.Models;

public class Cart
{
    public required Guid Id { get; set; }
    public required DateTime Created { get; set; }
    // public required virtual Shop Shop { get; set; }
    // public required virtual CartStateEnum CartState { get; set; }
    // public required virtual bool Paid { get; set; }
    public required virtual HashSet<CartItem> CartItems { get; set; }
    public required virtual HashSet<ShopItemProperty> Properties { get; set; }

}

// public enum CartStateEnum
// {
//     Unpaid,
//     Paid
// }

//customerstate:
//created
//hasactivatedpayment
//hascompletedpayment
//försök göra om till true/false
