
using PetShopApi.Models;
using shoppetApi.Helper;
using shoppetApi.Interfaces;
using shoppetApi.MyUnitOfWork;

namespace shoppetApi.Services
{
    public class BreedService : IBreedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBreedRepository _breedRepository;
        public BreedService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _breedRepository = unitOfWork.Breeds;
        }

        public Task<bool> BreedExists(string name)
        {
            return _breedRepository.BreedAlreadyExists(name);
        }

        public Task<bool> SpeciesExists(int id)
        {
            return _breedRepository.SpeciesIdExists(id);
        }

        public async Task<APIResponse<IEnumerable<Breed>>> GetSameSpeciesBreeds(int id)
        {
            var dataExists = await SpeciesExists(id);
            var result = await _breedRepository.GetSameSpeciesBreeds(id);
            if (result == null || !dataExists)
            {
                return APIResponse<IEnumerable<Breed>>.CreateResponse(false, MessageHelper.NotFound(typeof(Breed).Name), null);
            }
            return APIResponse<IEnumerable<Breed>>.CreateResponse(true, MessageHelper.Success(typeof(Breed).Name, MessageConstants.fetchedMessage), result);
 
        }
    }
}
