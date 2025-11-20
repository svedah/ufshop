namespace ufshop.Data.Models;

public class ShopContactInfo
{
    public required Guid Id { get; set; }

    public required string Email { get; set; }
    public required string MobileNumber { get; set; }
    public required virtual ShopSocialMedia SocialMedia { get; set; }

}