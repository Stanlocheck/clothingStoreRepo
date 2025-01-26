using System;
using ClothDTOs;

namespace clothingStoreWebAPI.Controllers;

public class RegisterModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public BuyerAddDTO BuyerInfo { get; set; }
}
