using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;

namespace shoppetApi.Services
{
    public interface IBreedService
    {
        public Task<APIResponse<IEnumerable<Breed>>> GetSameSpeciesBreeds(int id);
        public Task<APIResponse<Breed>> Add(BreedDTO breedDTO);
        public Task<APIResponse<Breed>> Update(string id, BreedDTO breedDTO);
    }
}
