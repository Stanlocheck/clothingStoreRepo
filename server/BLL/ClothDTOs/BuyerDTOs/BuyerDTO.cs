namespace ClothDTOs;

public class BuyerDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime DateOfReg { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Sex { get; set; }
    public string PhoneNumber { get; set; }
    public string City { get; set; }
    public string StreetAddress { get; set; }
    public int ApartmentNumber { get; set; }
    public int Balance { get; set; }
    public string Role { get; set; } = "Buyer";
}
