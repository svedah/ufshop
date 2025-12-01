namespace ufshop.Data.Models;

public class ShopOrder
{
    public required Guid Id { get; set; }
    public required ShopOrderStatus ShopOrderStatus { get; set; }
    // public required virtual Shop Shop { get; set; }
    public required virtual Customer Customer { get; set; }
}

public enum ShopOrderStatus
{
    None = 0,
    Paid = 1,
    Shipped = 2,
    Done = 3
}