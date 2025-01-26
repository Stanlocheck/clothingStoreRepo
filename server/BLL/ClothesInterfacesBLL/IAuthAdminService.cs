using System;
using ClothDTOs;

namespace ClothesInterfacesBLL;

public interface IAuthAdminService
{
    Task Register(string email, string password, AdminAddDTO adminInfo);
    Task Login(string email, string password);
    Task Logout();  
    public bool IsPasswordValid(string password);
    public bool IsEmailValid(string email);  
}
