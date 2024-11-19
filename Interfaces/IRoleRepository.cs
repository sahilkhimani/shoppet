using PetShopApi.Models;

namespace shoppetApi.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role> GetRole(string roleId);
    }
}
