namespace ufshop.Data.Models;

public class ShopPage
{
    public required Guid Id { get; set; }

    public required string Header { get; set; }
    public required int Order { get; set; }
    public required virtual HashSet<ShopPageFragment> ShopPageFragments { get; set; }
}