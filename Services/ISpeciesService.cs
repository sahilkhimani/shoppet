using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;

namespace shoppetApi.Services
{
    public interface ISpeciesService
    {
        public Task<APIResponse<Species>> Add(SpeciesDTO speciesDTO);
        public Task<APIResponse<Species>> Update(string id, SpeciesDTO speciesDTO);

    }
}
