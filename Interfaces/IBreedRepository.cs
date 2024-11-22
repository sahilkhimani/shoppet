using PetShopApi.Models;

namespace shoppetApi.Interfaces
{
    public interface IBreedRepository : IGenericRepository<Breed>
    {
        public Task<bool> SpeciesIdExists(int id);
        public Task<bool> BreedAlreadyExists(string name);
        public Task<IEnumerable<Breed>> GetSameSpeciesBreeds(int id);
        public Task<bool> BreedIdAlreadyExists(int id);


    }
}
