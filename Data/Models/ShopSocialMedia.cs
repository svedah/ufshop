namespace ufshop.Data.Models;

public class ShopSocialMedia
{
    public Guid Id { get; set; }

    public required string Facebook { get; set; }
    public required string Instagram { get; set; }
    public required string LinkedIn { get; set; }
    public required string TikTok { get; set; }
    public required string YouTube { get; set; }
}