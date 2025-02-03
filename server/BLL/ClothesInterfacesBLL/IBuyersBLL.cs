using System;
using ClothDTOs;

namespace ClothesInterfacesBLL;

public interface IBuyersBLL
{
    public Task<List<BuyerDTO>> GetAll();
    public Task<BuyerDTO> GetById(Guid id);
    public Task UpdateBuyer(BuyerUpdateDTO buyerUpdt);  
}
