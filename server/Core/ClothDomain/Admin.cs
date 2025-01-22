using System;
using Microsoft.AspNetCore.Identity;

namespace ClothDomain;

public class Admin : IdentityUser
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
