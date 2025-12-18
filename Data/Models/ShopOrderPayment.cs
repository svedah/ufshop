namespace ufshop.Data.Models;

public class ShopOrderPayment
{
    public required Guid Id { get; set; }
    public required DateTime Created { get; set; }
    public required int ConfirmedAmount { get; set; }

}