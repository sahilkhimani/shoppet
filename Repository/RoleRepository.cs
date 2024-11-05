using Microsoft.Identity.Client;
using PetShopApi.Data;
using PetShopApi.Models;
using shoppetApi.Interfaces;

namespace shoppetApi.Repository
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApiDbContext context) : base(context)
        { 
        }
        
    }
}
