using System;
using AutoMapper;
using ClothDomain;
using ClothesInterfacesBLL;
using ClothesInterfacesDAL;
using ClothDTOs;

namespace ClothingStoreApplication;

public class AdminBusiness : IAdminsBLL
{
    private readonly IAdminsDAO _adminsDAO;
    private Mapper _adminDTO;
    private Mapper _adminAddDTO;

    public AdminBusiness(IAdminsDAO adminsDAO){
        _adminsDAO = adminsDAO;

        var _adminDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Admin, AdminDTO>().ReverseMap());
        _adminDTO = new Mapper(_adminDtoMapping);

        var _adminAddDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Admin, AdminAddDTO>().ReverseMap());
        _adminAddDTO = new Mapper(_adminAddDtoMapping);
    }

     public async Task AddAdmin(AdminAddDTO admin){
        try{
            var adminAddDTO = _adminAddDTO.Map<AdminAddDTO, Admin>(admin);
            await _adminsDAO.AddAdmin(adminAddDTO);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
     }

     public async Task UpdateAdmin(AdminAddDTO admin, Guid id){
        try{
            var adminUpdateDTO = _adminAddDTO.Map<AdminAddDTO, Admin>(admin);
            await _adminsDAO.UpdateAdmin(adminUpdateDTO, id);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
     }

     public async Task DeleteAdmin(Guid id){
        try{
            await _adminsDAO.DeleteAdmin(id);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
     } 
}
