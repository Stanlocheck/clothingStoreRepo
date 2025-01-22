using System;
using Microsoft.AspNetCore.Identity;

namespace clothingStoreWebAPI.AuthControllers;

public class RegisterModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string City { get; set; }
    public string StreetAddress { get; set; }
    public string? ApartmentNumber { get; set; }
}
