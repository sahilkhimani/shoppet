using PetShopApi.Data;
using PetShopApi.Models;
using shoppetApi.Interfaces;

namespace shoppetApi.Repository
{
    public class BreedRepository : GenericRepository<Breed>, IBreedRepository
    {
        public BreedRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
