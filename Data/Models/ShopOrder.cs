namespace ufshop.Data.Models;

public class ShopOrder
{
    public required Guid Id { get; set; }
    public required DateTime Created { get; set; }
    public required virtual Cart Cart { get; set; }
    public required virtual CustomerInfo CustomerInfo { get; set; }
    public required virtual ShopOrderStatus Status { get; set; }

    // Loggning av Swish-betalning för Shop Order.
    // Servern sköter denna helt. Shopägare kan inte ändra denna
    public virtual ShopOrderPayment? Payment { get; set; }
}

public enum ShopOrderStatus
{
    Unpaid = 0,
    Paid = 1,
    Shipped = 2,
    Rejected = 3,
}