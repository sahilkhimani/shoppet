using Microsoft.Identity.Client;
using PetShopApi.Data;
using PetShopApi.Models;
using shoppetApi.Interfaces;

namespace shoppetApi.Repository
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly ApiDbContext _context;
        public RoleRepository(ApiDbContext context) : base(context)
        { 
            _context = context;
        }

        public async Task<Role> GetRole(int roleId)
        {
           return await _context.FindAsync<Role>(roleId);  
        }
    }
}
