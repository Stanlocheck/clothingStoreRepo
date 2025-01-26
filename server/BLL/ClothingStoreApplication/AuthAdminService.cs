using System;
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
    private Mapper _adminAddDTO;

    public AuthAdminService(IAdminsDAO adminsDAO, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor){
        _adminsDAO = adminsDAO;
        _context = context;
        _httpContextAccessor = httpContextAccessor;

        var _adminDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Admin, AdminDTO>().ReverseMap());
        _adminDTO = new Mapper(_adminDtoMapping);

        var _adminAddDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Admin, AdminAddDTO>().ReverseMap());
        _adminAddDTO = new Mapper(_adminAddDtoMapping);
    }

    public async Task Register(string email, string password, AdminAddDTO adminInfo){
        if(await _context.Admins.AnyAsync(a => a.Email == email) || await _context.Buyers.AnyAsync(b => b.Email == email)){
            throw new Exception("User with this email already exists.");
        }

        if(!IsPasswordValid(password)){
            throw new Exception("Password must contain 8 characters, at least one uppercase letter and one digit.");
        }

        var admin = new AdminAddDTO {
            FirstName = adminInfo.FirstName,
            LastName = adminInfo.LastName,
            Email = email,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            PhoneNumber = adminInfo.PhoneNumber,
        };
        var adminAddDTO = _adminAddDTO.Map<AdminAddDTO, Admin>(admin);
        await _adminsDAO.AddAdmin(adminAddDTO);
    }

    public async Task Login(string email, string password){
        var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
        if (admin == null || !BCrypt.Net.BCrypt.Verify(password, admin.Password))
        {
            throw new Exception("Invalid email or password.");
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
    public bool IsPasswordValid(string password){
        var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d).{8,}$");
        return regex.IsMatch(password);
    }
}
