using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;

namespace shoppetApi.Services
{
    public interface IPetService
    {
        public Task<APIResponse<IEnumerable<Pet>>> GetPetsByBreedId(int id);
        public Task<APIResponse<IEnumerable<Pet>>> GetPetsByGender(string gender);
        public Task<APIResponse<IEnumerable<Pet>>> GetPetsByAge(int age);
        public Task<APIResponse<IEnumerable<Pet>>> GetPetsByAgeRange(int minAge, int maxAge);
        public Task<APIResponse<IEnumerable<Pet>>> GetYourPets();
        public Task<APIResponse<Pet>> Add(PetDTO petDTO);
        public Task<APIResponse<Pet>> Update(string id, PetDTO petDTO);
        public Task<APIResponse<Pet>> Delete(string id);
    }
}