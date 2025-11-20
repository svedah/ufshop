namespace ufshop.Data.Models;
public class ShopItemProperty
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required bool Uploadable { get; set; }
    public required virtual HashSet<ShopItemPropertyOption> Options { get; set; }
    public required virtual ShopImage Image { get; set; }

}
