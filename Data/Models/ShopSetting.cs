namespace ufshop.Data.Models;

public class ShopSetting
{
    public required Guid Id { get; set; }

    public required string Title { get; set; }
    public required string SwishNumber { get; set; }
    public required int BaseShippingPrice { get; set; }
    public required string Description { get; set; }
    public required string Layout { get; set; }
    public required string Theme { get; set; }

    public required virtual ShopContactInfo ContactInfo { get; set; }

    public required virtual ShopImage LogoImage { get; set; }

}