namespace ufshop.Data.Models;

public class CustomerInfo
{
    public required Guid Id { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string StreetName { get; set; }
    public required string StreetNo { get; set; }
    public required string ZipCode { get; set; }
    public required string City { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Info { get; set; }
}