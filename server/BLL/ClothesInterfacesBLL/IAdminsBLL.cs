using System;
using ClothDTOs;

namespace ClothesInterfacesBLL;

public interface IAdminsBLL
{
    public Task AddAdmin(AdminAddDTO admin);
    public Task UpdateAdmin(AdminAddDTO admin, Guid id);
    public Task DeleteAdmin(Guid id);    
}
