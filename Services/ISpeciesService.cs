using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;

namespace shoppetApi.Services
{
    public interface ISpeciesService 
    {
        public Task<bool> AlreadyExists(string name);
    }
}
