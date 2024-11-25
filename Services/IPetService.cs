using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Interfaces;

namespace shoppetApi.Services
{
    public interface IPetService
    {
        public Task<bool> BreedIdAlreadyExists(int id);
        public Task<string> CheckUser(PetDTO petDTO);
        public Task<string> CheckOwnPets(string id);
        public Task<APIResponse<IEnumerable<Pet>>> GetPetsByBreedId(int id);
        public Task<APIResponse<IEnumerable<Pet>>> GetPetsByGender(string gender);
        public Task<APIResponse<IEnumerable<Pet>>> GetPetsByAge(int age);
        public Task<APIResponse<IEnumerable<Pet>>> GetPetsByAgeRange(int minAge, int maxAge);
        public Task<APIResponse<IEnumerable<Pet>>> GetYourPets();
    }

}
