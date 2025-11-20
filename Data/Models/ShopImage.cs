namespace ufshop.Data.Models;

public class ShopImage
{
    public required Guid Id { get; set; }
    public required string Filename { get; set; }
    public required string AltText { get; set; }
    public required DateTime Created { get; set; }
}