using System;
using ClothDomain;
using ClothesInterfacesDAL;
using Microsoft.EntityFrameworkCore;

namespace ClothingStorePersistence;

public class SqlDAO : IClothesDAO
{
    private readonly ApplicationDbContext _context;

    public SqlDAO(ApplicationDbContext context){
        _context = context;
    }
    
    public async Task<List<Cloth>> GetAll(){
        return await _context.Clothes.ToListAsync();
    }
    public async Task<Cloth> GetById(Guid id){
        var cloth = _context.Clothes.Where(_cloth => _cloth.Id == id);
        if(cloth == null){
            throw new Exception("Object not found.");
        }

        return await cloth.FirstOrDefaultAsync();
    }
    public async Task AddCloth(Cloth cloth){
        await _context.Clothes.AddAsync(cloth);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateCloth(Cloth clothUpdt, Guid id){
        var cloth = await _context.Clothes.FindAsync(id);  
        if(cloth == null){
            throw new Exception("Object not found.");
        } 

        cloth.Price = clothUpdt.Price;
        cloth.Size = clothUpdt.Size;
        cloth.Manufacturer = clothUpdt.Manufacturer;
        cloth.Brand = clothUpdt.Brand;
        cloth.Material = clothUpdt.Material;
        cloth.Season = clothUpdt.Season;
        cloth.Type = clothUpdt.Type;     
    }
    public async Task DeleteCloth(Guid id){
        var cloth = await _context.Clothes.FindAsync(id);
        if(cloth == null){
            throw new Exception("Object not found.");
        }

        _context.Clothes.Remove(cloth);
        await _context.SaveChangesAsync();
    }
}
