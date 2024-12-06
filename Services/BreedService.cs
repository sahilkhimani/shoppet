using AutoMapper;
using Humanizer;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Interfaces;
using shoppetApi.MyUnitOfWork;

namespace shoppetApi.Services
{
    public class BreedService : IBreedService
    {
        public const string AlreadyExistsBreedMessage = "Breed Already Exists";
        public const string NotExistsSpeciesMessage = "Species Not Exists";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBreedRepository _breedRepository;
        private readonly IMapper _mapper;

        public BreedService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _breedRepository = unitOfWork.Breeds;
            _mapper = mapper;   
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
            try
            {
                if(id <= 0) return APIResponse<IEnumerable<Breed>>.CreateResponse(false, MessageConstants.InvalidId, null);
                var dataExists = await SpeciesExists(id);
                var result = await _breedRepository.GetSameSpeciesBreeds(id);
                if (!result.Any() || !dataExists)
                {
                    return APIResponse<IEnumerable<Breed>>.CreateResponse(false, MessageHelper.NotFound(nameof(Breed)), null);
                }
                return APIResponse<IEnumerable<Breed>>.CreateResponse(true, MessageHelper.Success(nameof(Breed), MessageConstants.fetchedMessage), result);
            }
            catch (Exception ex)
            {
                return APIResponse<IEnumerable<Breed>>.CreateResponse(false, MessageHelper.Exception(nameof(Breed), MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<Breed>> BreedAndSpeciesExists(BreedDTO breedDTO)
        {
            var speciesExists = await SpeciesExists(breedDTO.SpeciesId);
            if (!speciesExists) return APIResponse<Breed>.CreateResponse(false, NotExistsSpeciesMessage, null);
            var breedExists = await BreedExists(breedDTO.BreedName);
            if (breedExists) return APIResponse<Breed>.CreateResponse(false, AlreadyExistsBreedMessage, null);
            return APIResponse<Breed>.CreateResponse(true, "ok", null);
        }

        public async Task<APIResponse<Breed>> Add(BreedDTO breedDTO)
        {
            try
            {
                var checkBreedAndSpecies = await BreedAndSpeciesExists(breedDTO);
                if (!checkBreedAndSpecies.Success) return checkBreedAndSpecies;

                breedDTO.BreedName = HelperMethods.ApplyTitleCase(breedDTO.BreedName);
                var data = _mapper.Map<Breed>(breedDTO);

                await _breedRepository.Add(data);
                await _unitOfWork.SaveAsync();
                return APIResponse<Breed>.CreateResponse(true, MessageHelper.Success(nameof(Breed), MessageConstants.createdMessage), data);
            }
            catch (Exception ex)
            {
                return APIResponse<Breed>.CreateResponse(false, MessageHelper.Exception(nameof(Breed), MessageConstants.creatingMessage, ex.Message), null);
            }
        }
        public async Task<APIResponse<Breed>> Update(string id, BreedDTO breedDTO)
        {
            try
            {
                var parsedId = HelperMethods.ParseId(id);
                if (parsedId == null) return APIResponse<Breed>.CreateResponse(false, MessageConstants.InvalidId, null);
                var data = await _breedRepository.GetById(parsedId);
                if (data == null) return APIResponse<Breed>.CreateResponse(false, MessageHelper.NotFound(nameof(Breed)), null);

                var checkBreedAndSpecies = await BreedAndSpeciesExists(breedDTO);
                if (!checkBreedAndSpecies.Success) return checkBreedAndSpecies;

                breedDTO.BreedName = HelperMethods.ApplyTitleCase(breedDTO.BreedName);
                var updatedData = _mapper.Map(breedDTO, data);

                await _breedRepository.Update(parsedId, data);
                await _unitOfWork.SaveAsync();
                return APIResponse<Breed>.CreateResponse(true, MessageHelper.Success(nameof(Breed), MessageConstants.updatedMessage), data);
            }
            catch (Exception ex)
            {
                return APIResponse<Breed>.CreateResponse(false, MessageHelper.Exception(nameof(Breed), MessageConstants.updatingMessage, ex.Message), null);
            }
        }
    }
}
