namespace ufshop.Data.Models;

public class ShopPageFragment
{
    public required Guid Id { get; set; }

    public required string Header { get; set; }
    public required string Paragraph { get; set; }
    public required int Order { get; set; }
    public virtual ShopImage? Image { get; set; }
}