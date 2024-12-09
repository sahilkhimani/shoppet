using Microsoft.EntityFrameworkCore;
using PetShopApi.Data;
using PetShopApi.Models;
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
            var ownerId = _context.Pets
                .Where(x => x.PetId == id)
                .Select(x => x.OwnerId)
                .FirstOrDefault();
            return ownerId;
        }

        public async Task<IEnumerable<Pet>> GetPetsByAge(int age)
        {
            return await _context.Pets
                .AsNoTracking()
                .Where(x => x.PetAge == age)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetPetsByAgeRange(int minAge, int maxAge)
        {
            return await _context.Pets
                .AsNoTracking()
                .Where(x => x.PetAge >= minAge && x.PetAge <= maxAge)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetPetsByBreedId(int id)
        {
            return await _context.Pets
                .AsNoTracking()
                .Where(x => x.BreedId == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetPetsByGender(string gender)
        {
            return await _context.Pets
                .AsNoTracking()
                .Where(x => x.PetGender == gender)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetYourPets(string id)
        {
            return await _context.Pets
                .AsNoTracking()
                .Where(x => x.OwnerId == id)
                .ToListAsync();
        }
    }
}
