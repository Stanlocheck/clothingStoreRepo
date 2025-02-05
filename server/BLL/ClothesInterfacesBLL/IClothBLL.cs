using System;
using ClothDomain;
using ClothDTOs;

namespace ClothesInterfacesBLL;

public interface IClothBLL
{
    public Task<List<ClothDTO>> GetAll();
    public Task<ClothDTO> GetById(Guid id);
    public Task AddCloth(ClothAddDTO cloth);
    public Task UpdateCloth(ClothAddDTO clothUpdt, Guid id);
    public Task DeleteCloth(Guid id);
    public Task<List<ClothDTO>> GetMensClothing();
    public Task<List<ClothDTO>> GetWomensClothing();
}
