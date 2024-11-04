using PetShopApi.Data;
using PetShopApi.Models;
using shoppetApi.Interfaces;

namespace shoppetApi.Repository
{
    public class SpeciesRepository : GenericRepository<Species>, ISpeciesRepository
    {
        public SpeciesRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
