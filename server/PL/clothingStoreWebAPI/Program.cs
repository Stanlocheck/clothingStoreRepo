using Microsoft.EntityFrameworkCore;
using ClothingStorePersistence;
using Microsoft.OpenApi.Models;
using ClothesInterfacesBLL;
using ClothesInterfacesDAL;
using ClothingStoreApplication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connection));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/api/auth/login";
        options.LogoutPath = "/api/auth/logout";
        options.AccessDeniedPath = "/api/auth/access-denied";
    });
builder.Services.AddAuthorization(options => {
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("BuyerOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Buyer"));
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Clothing Store",
        Description = "Simple ASP NET project",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<IClothesDAO, SqlDAO>();
builder.Services.AddScoped<IClothBLL, ClothBusiness>();

builder.Services.AddScoped<ICartDAO, SqlCartDAO>();
builder.Services.AddScoped<ICartBLL, CartBusiness>();

builder.Services.AddScoped<IWishlistDAO, SqlWishlistDAO>();
builder.Services.AddScoped<IWishlistBLL, WishlistBusiness>();

builder.Services.AddScoped<IBuyersDAO, SqlBuyersDAO>();
builder.Services.AddScoped<IBuyersBLL, BuyerBusiness>();

builder.Services.AddScoped<IAdminsDAO, SqlAdminsDAO>();
builder.Services.AddScoped<IAdminsBLL, AdminBusiness>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthAdminService, AuthAdminService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
