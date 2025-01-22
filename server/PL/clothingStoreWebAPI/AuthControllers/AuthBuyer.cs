using ClothDomain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace clothingStoreWebAPI.AuthControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthBuyer : ControllerBase
    {
    private readonly UserManager<Buyer> _userManager;
    private readonly IConfiguration _configuration;

    public AuthBuyer(UserManager<Buyer> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var buyer = new Buyer { FirstName = model.FirstName, LastName = model.LastName, Email = model.Email, DateOfBirth = model.DateOfBirth, Gender = model.Gender, 
                                PhoneNumber = model.PhoneNumber, City = model.City, StreetAddress = model.StreetAddress, ApartmentNumber = model.ApartmentNumber };
        var result = await _userManager.CreateAsync(buyer, model.Password);

        if (result.Succeeded)
        {
            return Ok(new { message = "User registered successfully" });
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var buyer = await _userManager.FindByEmailAsync(model.Email);
        if (buyer != null && await _userManager.CheckPasswordAsync(buyer, model.Password))
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, buyer.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:ValidIssuer"],
                audience: _configuration["JwtSettings:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        return Unauthorized();
    }      
    }
}
