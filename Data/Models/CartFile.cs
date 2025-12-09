
namespace ufshop.Data.Models;

public class CartFile
{
    public required Guid Id { get; set; }
    public required string Url { get; set; }
    public required string Filename { get; set; }
}