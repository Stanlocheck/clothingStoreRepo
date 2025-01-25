using System;
using ClothDTOs;

namespace ClothesInterfacesBLL;

public interface IAdminsBLL
{
    public Task<List<AdminDTO>> GetAll();
    public Task<AdminDTO> GetById(Guid id);
    public Task AddAdmin(AdminAddDTO admin);
    public Task UpdateAdmin(AdminAddDTO admin, Guid id);
    public Task DeleteAdmin(Guid id);    
}
