using System;
using Microsoft.AspNetCore.Identity;

namespace clothingStoreWebAPI.AuthControllers;

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}
