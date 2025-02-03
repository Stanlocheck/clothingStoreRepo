using System;
using AutoMapper;
using ClothDomain;
using ClothesInterfacesBLL;
using ClothesInterfacesDAL;
using ClothDTOs;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ClothingStoreApplication;

public class AdminBusiness : IAdminsBLL
{
    private readonly IAdminsDAO _adminsDAO;
    private Mapper _adminDTO;
    private Mapper _adminAddDTO;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AdminBusiness(IAdminsDAO adminsDAO, IHttpContextAccessor httpContextAccessor){
        _adminsDAO = adminsDAO;
        _httpContextAccessor = httpContextAccessor;

        var _adminDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Admin, AdminDTO>().ReverseMap());
        _adminDTO = new Mapper(_adminDtoMapping);

        var _adminAddDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Admin, AdminAddDTO>().ReverseMap());
        _adminAddDTO = new Mapper(_adminAddDtoMapping);
    }

    private Guid GetLoggedInBuyerId(){
        var httpContext = _httpContextAccessor.HttpContext;
        if(httpContext == null){
            throw new Exception("HttpContext недоступен.");
        }

        var adminIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if(adminIdClaim == null || !Guid.TryParse(adminIdClaim.Value, out var adminId)){
            throw new Exception("Пользователь не авторизован.");
        }

        return adminId;
    }

    public async Task UpdateAdmin(AdminAddDTO admin){
        try{
            var adminId = GetLoggedInBuyerId();

            var adminUpdateDTO = _adminAddDTO.Map<AdminAddDTO, Admin>(admin);
            await _adminsDAO.UpdateAdmin(adminUpdateDTO, adminId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }
}
