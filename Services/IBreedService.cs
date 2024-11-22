using PetShopApi.Models;
using shoppetApi.Helper;

namespace shoppetApi.Services
{
    public interface IBreedService
    {
        public Task<bool> SpeciesExists(int id);
        public Task<bool> BreedExists(string name);
        public Task<APIResponse<IEnumerable<Breed>>> GetSameSpeciesBreeds(int id);

    }
}
