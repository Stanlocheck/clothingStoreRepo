using System;
using ClothDomain;
using ClothDTOs;

namespace ClothesInterfacesBLL;

public interface IAuthService
{
    Task Register(BuyerAddDTO buyerInfo);
    Task Login(string email, string password);
    Task Logout();
    public bool IsPasswordValid(string password);
    public bool IsEmailValid(string email);
    public bool IsPhoneNumberValid(string phoneNumber);
}
