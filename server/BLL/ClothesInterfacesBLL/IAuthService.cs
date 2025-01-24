using System;
using ClothDomain;
using ClothDTOs;

namespace ClothesInterfacesBLL;

public interface IAuthService
{
    Task Register(string email, string password, BuyerAddDTO buyerInfo);
    Task Login(string email, string password);
    Task Logout();
}
