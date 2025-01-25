using System;
using ClothDTOs;

namespace clothingStoreWebAPI.Controllers;

public class RegisterAdminModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public AdminAddDTO AdminInfo { get; set; }
}
