using ClothDomain;

namespace ClothesInterfacesDAL;

public interface IAdminsDAO
{
    public Task<List<Admin>> GetAll();
    public Task<Admin> GetById(Guid id);
    public Task<Admin?> GetByEmail(string email);
    public Task AddAdmin(Admin admin);
    public Task UpdateAdmin(Admin admin, Guid id);
    public Task DeleteAdmin(Guid id);
}
