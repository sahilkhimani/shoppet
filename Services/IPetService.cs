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
        public Task<bool> CheckOwnPets(string id);

    }

}
