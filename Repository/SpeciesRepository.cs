using Microsoft.EntityFrameworkCore;
using PetShopApi.Data;
using PetShopApi.Models;
using shoppetApi.Interfaces;

namespace shoppetApi.Repository
{
    public class SpeciesRepository : GenericRepository<Species>, ISpeciesRepository
    {
        private readonly ApiDbContext _context;
        public SpeciesRepository(ApiDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> SpeciesAlreadyExists(string name)
        {
            return await _context.Species.AnyAsync(x => x.SpeciesName.ToLower() == name.ToLower());
        }
    }
}
