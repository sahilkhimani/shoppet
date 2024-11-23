using PetShopApi.Models;

namespace shoppetApi.Interfaces
{
    public interface IPetRepository : IGenericRepository<Pet>
    {
        public string GetOwnerOnPetId(int id);
    }
}
