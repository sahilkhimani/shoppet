using Microsoft.EntityFrameworkCore;
using PetShopApi.Data;
using PetShopApi.Models;
using shoppetApi.Interfaces;

namespace shoppetApi.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApiDbContext _context;
        public UserRepository(ApiDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {

            return await _context.Users.SingleOrDefaultAsync(u => u.UserEmail == email);

        }
    }
}
