namespace ClothDTOs;

public class BuyerUpdateDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Sex { get; set; }
    public string PhoneNumber { get; set; }
    public string City { get; set; }
    public string StreetAddress { get; set; }
    public int ApartmentNumber { get; set; }
}