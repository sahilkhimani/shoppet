using PetShopApi.Models;

namespace shoppetApi.Interfaces
{
    public interface ISpeciesRepository : IGenericRepository<Species>
    {
        public Task<bool> SpeciesAlreadyExists(string name);

    }
}
