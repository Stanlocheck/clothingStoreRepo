using System;
using ClothDTOs;

namespace ClothesInterfacesBLL;

public interface IAdminsBLL
{
    public Task UpdateAdmin(AdminAddDTO admin);   
}
