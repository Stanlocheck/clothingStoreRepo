using Microsoft.EntityFrameworkCore;
using ClothingStorePersistence;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connection));

app.Run();
