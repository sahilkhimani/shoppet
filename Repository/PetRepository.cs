using PetShopApi.Data;
using PetShopApi.Models;
using shoppetApi.Helper;
using shoppetApi.Interfaces;

namespace shoppetApi.Repository
{
    public class PetRepository : GenericRepository<Pet>, IPetRepository
    {
        private readonly ApiDbContext _context;
        public PetRepository(ApiDbContext context) : base(context) 
        {
            _context = context;
        }

        public string GetOwnerOnPetId(int id)
        {
            return _context.Pets
                .Where(x => x.PetId == id)
                .Select(x => x.OwnerId)
                .First();
        }

    }
}
