using PetShopApi.Models;

namespace shoppetApi.Interfaces
{
    public interface IPetRepository : IGenericRepository<Pet>
    {
        public string GetOwnerOnPetId(int id);
        public Task<IEnumerable<Pet>> GetPetsByBreedId(int id);
        public Task<IEnumerable<Pet>> GetPetsByAge(int age);
        public Task<IEnumerable<Pet>> GetPetsByAgeRange(int minAge, int maxAge);
        public Task<IEnumerable<Pet>> GetPetsByGender(string gender);
        public Task<IEnumerable<Pet>> GetYourPets(string id);
        public Task<bool> PetExists(string id);
    }
}
