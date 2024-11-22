using PetShopApi.Models;

namespace shoppetApi.Interfaces
{
    public interface IPetRepository : IGenericRepository<Pet>
    {
        public Task<string> GetOwnerOnPetId(int id);
    }
}
