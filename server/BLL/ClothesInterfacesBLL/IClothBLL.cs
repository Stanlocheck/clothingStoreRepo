using System;
using ClothDTOs;

namespace ClothesInterfacesBLL;

public interface IClothBLL
{
    public Task<List<ClothDTO>> GetAll();
    public Task<ClothDTO> GetById(Guid id);
    public Task AddCloth(ClothAddDTO cloth);
    public Task UpdateCloth(ClothAddDTO clothUpdt);
    public Task DeleteCloth(Guid id);
}
