

namespace ufshop.Data.Models;

public class UFShopOrder
{
    public required Guid Id { get; set; }
    public required string Prefix { get; set; }
    public required string Email { get; set; }
    public required string Title { get; set; }
    public required bool UF { get; set; }
    public required bool Assisted { get; set; }
    public required bool Active { get; set; }
    public required bool Paid { get; set; }
    public required DateTime Created { get; set; }
}
