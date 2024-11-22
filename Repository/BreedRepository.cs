using Microsoft.EntityFrameworkCore;
using PetShopApi.Data;
using PetShopApi.Models;
using shoppetApi.Helper;
using shoppetApi.Interfaces;

namespace shoppetApi.Repository
{
    public class BreedRepository : GenericRepository<Breed>, IBreedRepository
    {
        private readonly ApiDbContext _context;
        public BreedRepository(ApiDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> BreedAlreadyExists(string name)
        {
            return await _context.Breeds.AnyAsync(x=> x.BreedName.ToLower() == name.ToLower());
        }

        public async Task<bool> SpeciesIdExists(int id)
        {
            return await _context.Species.AnyAsync(x=>x.SpeciesId == id);
        }

        public async Task<IEnumerable<Breed>> GetSameSpeciesBreeds(int id)
        {
            return await _context.Breeds
                .Where(x => x.SpeciesId == id)
                .ToListAsync();
        }

    }
}
