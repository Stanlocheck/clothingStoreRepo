namespace ClothDTOs;

public class BuyerAddDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Sex { get; set; }
    public string PhoneNumber { get; set; }
    public string City { get; set; }
    public string StreetAddress { get; set; }
    public int ApartmentNumber { get; set; }
    public int Balance { get; set; }
}
