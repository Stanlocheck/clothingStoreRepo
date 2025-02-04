using System;
using System.Text.Json.Serialization;

namespace ClothDTOs;

public class AdminAddDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
}
