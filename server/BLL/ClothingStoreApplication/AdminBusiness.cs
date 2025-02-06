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
    private Mapper _adminUpdateDTO;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AdminBusiness(IAdminsDAO adminsDAO, IHttpContextAccessor httpContextAccessor){
        _adminsDAO = adminsDAO;
        _httpContextAccessor = httpContextAccessor;

        var _adminUpdateDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Admin, AdminUpdateDTO>().ReverseMap());
        _adminUpdateDTO = new Mapper(_adminUpdateDtoMapping);
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

    public async Task UpdateAdmin(AdminUpdateDTO admin){
        try{
            var adminId = GetLoggedInBuyerId();

            var adminUpdateDTO = _adminUpdateDTO.Map<AdminUpdateDTO, Admin>(admin);
            await _adminsDAO.UpdateAdmin(adminUpdateDTO, adminId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }
}
