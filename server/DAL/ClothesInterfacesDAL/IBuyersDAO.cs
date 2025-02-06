using ClothDomain;

namespace ClothesInterfacesDAL;

public interface IBuyersDAO
{
    public Task<List<Buyer>> GetAll();
    public Task<Buyer> GetById(Guid id);
    public Task<Buyer?> GetByEmail(string email);
    public Task AddBuyer(Buyer buyer);
    public Task UpdateBuyer(Buyer buyer, Guid id);
    public Task DeleteBuyer(Guid id);
}
