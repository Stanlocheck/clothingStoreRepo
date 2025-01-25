using ClothDTOs;
using ClothesInterfacesBLL;
using ClothesInterfacesDAL;
using ClothingStorePersistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using AutoMapper;
using ClothDomain;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ClothingStoreApplication;

public class AuthService : IAuthService
{
    private readonly IBuyersDAO _buyersDAO;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Mapper _buyerDTO;
    private Mapper _buyerAddDTO;

    public AuthService(IBuyersDAO buyersDAO, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor){
        _buyersDAO = buyersDAO;
        _context = context;
        _httpContextAccessor = httpContextAccessor;

        var _buyerDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Buyer, BuyerDTO>().ReverseMap());
        _buyerDTO = new Mapper(_buyerDtoMapping);

        var _buyerAddDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Buyer, BuyerAddDTO>().ReverseMap());
        _buyerAddDTO = new Mapper(_buyerAddDtoMapping);
    }

    public async Task Register(string email, string password, BuyerAddDTO buyerInfo){
        if(await _context.Buyers.AnyAsync(b => b.Email == email)){
            throw new Exception("User with this email already exists.");
        }

        var buyer = new BuyerAddDTO {
            FirstName = buyerInfo.FirstName,
            LastName = buyerInfo.LastName,
            Email = email,
            Password = BCrypt.Net.BCrypt.HashPassword(password), // Хэширование пароля
            DateOfBirth = buyerInfo.DateOfBirth,
            Gender = buyerInfo.Gender,
            PhoneNumber = buyerInfo.PhoneNumber,
            City = buyerInfo.City,
            StreetAddress = buyerInfo.StreetAddress,
            ApartmentNumber = buyerInfo.ApartmentNumber,
        };
        var buyerAddDTO = _buyerAddDTO.Map<BuyerAddDTO, Buyer>(buyer);
        await _buyersDAO.AddBuyer(buyerAddDTO);
    }

    public async Task Login(string email, string password){
        var buyer = await _context.Buyers.FirstOrDefaultAsync(b => b.Email == email);
        if (buyer == null || !BCrypt.Net.BCrypt.Verify(password, buyer.Password))
        {
            throw new Exception("Invalid email or password.");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, buyer.Email),
            new Claim(ClaimTypes.NameIdentifier, buyer.Id.ToString()),
            new Claim(ClaimTypes.Role, buyer.Role)
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
}
