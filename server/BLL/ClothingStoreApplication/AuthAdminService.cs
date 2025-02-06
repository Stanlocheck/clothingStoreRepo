using ClothesInterfacesBLL;
using ClothesInterfacesDAL;
using Microsoft.EntityFrameworkCore;
using ClothingStorePersistence;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using ClothDomain;
using ClothDTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Text.RegularExpressions;

namespace ClothingStoreApplication;

public class AuthAdminService : IAuthAdminService
{
    private readonly IAdminsDAO _adminsDAO;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Mapper _adminDTO;

    public AuthAdminService(IAdminsDAO adminsDAO, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor){
        _adminsDAO = adminsDAO;
        _context = context;
        _httpContextAccessor = httpContextAccessor;

        var _adminDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Admin, AdminDTO>().ReverseMap());
        _adminDTO = new Mapper(_adminDtoMapping);
    }

    public async Task Register(AdminAddDTO adminInfo){
        if(await _context.Admins.AnyAsync(a => a.Email == adminInfo.Email)){
            throw new Exception("Пользователь с таким Email уже существует.");
        }

        if(!IsEmailValid(adminInfo.Email)){
            throw new Exception("Неверный формат Email.");
        }

        if(!IsPasswordValid(adminInfo.Password)){
            throw new Exception("Неверный формат пароля. Пароль должен содержать минимум 8 символов, 1 заглавную букву и 1 цифру.");
        }

        if(!IsPhoneNumberValid(adminInfo.PhoneNumber)){
            throw new Exception("Неверный формат номера телефона.");
        }

        try{
            var admin = new AdminDTO {
                Id = Guid.NewGuid(),
                FirstName = adminInfo.FirstName,
                LastName = adminInfo.LastName,
                Email = adminInfo.Email,
                DateOfReg = DateTime.UtcNow,
                DateOfBirth = adminInfo.DateOfBirth,
                Password = BCrypt.Net.BCrypt.HashPassword(adminInfo.Password),
                PhoneNumber = adminInfo.PhoneNumber,
                Role = "Admin"
            };
            var adminAddDTO = _adminDTO.Map<AdminDTO, Admin>(admin);
            await _adminsDAO.AddAdmin(adminAddDTO);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
        
    }

    public async Task Login(string email, string password){
        var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
        if (admin == null || !BCrypt.Net.BCrypt.Verify(password, admin.Password))
        {
            throw new Exception("Неверный Email или пароль.");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, admin.Email),
            new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
            new Claim(ClaimTypes.Role, admin.Role)
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties();

        await _httpContextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    public async Task Logout(){
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    private bool IsPasswordValid(string password){
        var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d).{8,}$");
        return regex.IsMatch(password);
    }
    private bool IsEmailValid(string email){
        var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        return regex.IsMatch(email);
    }
    private bool IsPhoneNumberValid(string phoneNumber){
        var regex = new Regex(@"^(?:\+7|8)?[\s\-]?\(?\d{3}\)?[\s\-]?\d{3}[\s\-]?\d{2}[\s\-]?\d{2}$");
        return regex.IsMatch(phoneNumber);
    }
}
