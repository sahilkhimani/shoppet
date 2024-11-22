
using PetShopApi.Models;
using shoppetApi.Helper;
using shoppetApi.MyUnitOfWork;

namespace shoppetApi.Services
{
    public class BreedService : IBreedService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BreedService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> BreedExists(string name)
        {
            return _unitOfWork.Breeds.BreedAlreadyExists(name);
        }

        public Task<bool> SpeciesExists(int id)
        {
            return _unitOfWork.Breeds.SpeciesIdExists(id);
        }

        public async Task<APIResponse<IEnumerable<Breed>>> GetSameSpeciesBreeds(int id)
        {
            try
            {
                var dataExists = await _unitOfWork.Breeds.SpeciesIdExists(id);
                var result = await _unitOfWork.Breeds.GetSameSpeciesBreeds(id);
                if (result == null || !dataExists)
                {
                    return APIResponse<IEnumerable<Breed>>.CreateResponse(false, MessageHelper.NotFound(typeof(Breed).Name), null);
                }
                return APIResponse<IEnumerable<Breed>>.CreateResponse(true, MessageHelper.Success(typeof(Breed).Name, "fetched"), result);
            }
            catch (Exception ex)
            {
                return APIResponse<IEnumerable<Breed>>.CreateResponse(false, MessageHelper.Exception(typeof(Breed).Name, "fetching", ex.Message), null);
            }
        }
    }
}
