using Microsoft.EntityFrameworkCore;
using ClothingStorePersistence;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connection));

var app = builder.Build();

app.MapGet("/", (ApplicationDbContext db) => db.Clothes.ToList());

app.Run();
