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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpeciesRepository _speciesRepository;
        public SpeciesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _speciesRepository = unitOfWork.Species;
        }

        public async Task<bool> AlreadyExists(string name)
        {
            return await _speciesRepository.SpeciesAlreadyExists(name);
        }
    }
}
