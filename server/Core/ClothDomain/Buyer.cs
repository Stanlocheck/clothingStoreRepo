using System.ComponentModel.DataAnnotations.Schema;

namespace ClothDomain;

public class Buyer
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateOnly DateOfBirth { get; set; }
    [Column(TypeName = "varchar(50)")]
    public Gender Sex { get; set; }
    public string? PhoneNumber { get; set; }
    public string City { get; set; }
    public string StreetAddress { get; set; }
    public int ApartmentNumber { get; set; }
    public string Role { get; set; } = "Buyer";
}
