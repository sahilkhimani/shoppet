using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Interfaces;
using shoppetApi.MyUnitOfWork;
using shoppetApi.Repository;

namespace shoppetApi.Services
{
    public class SpeciesService : ISpeciesService
    {
        private readonly ISpeciesRepository _speciesRepository;
        public SpeciesService(ISpeciesRepository speciesRepository)
        {
            _speciesRepository = speciesRepository;
        }

        public async Task<bool> AlreadyExists(string name)
        {
            return await _speciesRepository.SpeciesAlreadyExists(name);
        }
    }
}
