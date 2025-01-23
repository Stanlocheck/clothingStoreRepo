using System;
using ClothDTOs;

namespace ClothesInterfacesBLL;

public interface IBuyersBLL
{
    public Task<List<BuyerDTO>> GetAll();
    public Task<BuyerDTO> GetById(Guid id);
    public Task AddBuyer(BuyerAddDTO buyer);
    public Task UpdateBuyer(BuyerAddDTO buyerUpdt, Guid id);
    public Task DeleteBuyer(Guid id);    
}
