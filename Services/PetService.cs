using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Interfaces;
using shoppetApi.MyUnitOfWork;

namespace shoppetApi.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;
        private readonly IBreedRepository _breedRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextHelper _httpContextHelper;
        private readonly IMapper _mapper;

        public const string maleGender = "Male";
        public const string femaleGender = "Female";
        public const string NotExistsBreedMessage = "Breed Not Exists";
        public const string WrongGenderMessage = "Please Select the right gender";


        public PetService(IUnitOfWork unitOfWork, IHttpContextHelper httpContextHelper, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _petRepository = _unitOfWork.Pets;
            _breedRepository = _unitOfWork.Breeds;
            _httpContextHelper = httpContextHelper;
            _mapper = mapper;
        }

        public async Task<APIResponse<Pet>> Add(PetDTO petDTO)
        {
            try
            {
                var details = await ValidatePetDetails(petDTO);
                if (!details.Success) return details;

                var data = _mapper.Map<Pet>(petDTO);
                data.OwnerId = details.Message;

                await _petRepository.Add(data);
                await _unitOfWork.SaveAsync();
                return APIResponse<Pet>.CreateResponse(true, MessageHelper.Success(nameof(Pet), MessageConstants.createdMessage), null);
            }
            catch (Exception ex)
            {
                return APIResponse<Pet>.CreateResponse(false, MessageHelper.Exception(nameof(User), MessageConstants.creatingMessage, ex.Message), null);
            }
        }
        public async Task<APIResponse<Pet>> Update(string id, PetDTO petDTO)
        {
            try
            {
                var parsedId = HelperMethods.ParseId(id);
                if (parsedId == null) return APIResponse<Pet>.CreateResponse(false, MessageConstants.InvalidId, null);

                var data = await _petRepository.GetById(parsedId);
                if (data == null) return APIResponse<Pet>.CreateResponse(false, MessageConstants.DataNotFound, null);

                var ownPet = CheckOwnPets(parsedId);
                if (!ownPet.Success) return ownPet;

                var details = await ValidatePetDetails(petDTO);
                if (!details.Success) return details;

                var updatedData = _mapper.Map(petDTO, data);

                await _petRepository.Update(parsedId, updatedData);
                await _unitOfWork.SaveAsync();
                return APIResponse<Pet>.CreateResponse(true, MessageHelper.Success(nameof(Pet), MessageConstants.updatedMessage), null);
            }
            catch (Exception ex)
            {
                return APIResponse<Pet>.CreateResponse(false, MessageHelper.Exception(nameof(User), MessageConstants.updatingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<Pet>> Delete(string id)
        {
            try
            {
                var parsedId = HelperMethods.ParseId(id);
                if (parsedId == null) return APIResponse<Pet>.CreateResponse(false, MessageConstants.InvalidId, null);

                var data = await _petRepository.GetById(parsedId);
                if (data == null) return APIResponse<Pet>.CreateResponse(false, MessageConstants.DataNotFound, null);

                var ownPet = CheckOwnPets(parsedId);
                if (!ownPet.Success) return ownPet;

                await _petRepository.Delete(parsedId);
                await _unitOfWork.SaveAsync();
                return APIResponse<Pet>.CreateResponse(true, MessageHelper.Success(nameof(Pet), MessageConstants.deletedMessage), null);
            }
            catch (Exception ex)
            {
                return APIResponse<Pet>.CreateResponse(false, MessageHelper.Exception(nameof(User), MessageConstants.deletingMessage, ex.Message), null);
            }
        }

        private async Task<bool> BreedIdAlreadyExists(int id)
        {
            return await _breedRepository.BreedIdAlreadyExists(id);
        }

        private APIResponse<Pet> CheckOwnPets(object id)
        {
            int parsedId = Convert.ToInt32(id);
            var ownerId = _petRepository.GetOwnerOnPetId(parsedId);
            var currentUserId = _httpContextHelper.GetCurrentUserId();
            if (ownerId == null || ownerId != currentUserId)
            {
                return APIResponse<Pet>.CreateResponse(false, MessageConstants.UnAuthorizedUserMessage, null);
            }
            return APIResponse<Pet>.CreateResponse(true, ownerId, null);
        }

        private async Task<APIResponse<Pet>> ValidatePetDetails(PetDTO petDTO)
        {
            var userId = _httpContextHelper.GetCurrentUserId();
            if (userId == null) return APIResponse<Pet>.CreateResponse(false, MessageConstants.InvalidId, null);

            var breedExists = await BreedIdAlreadyExists(petDTO.BreedId);
            if (!breedExists) return APIResponse<Pet>.CreateResponse(false, NotExistsBreedMessage, null);

            var petGender = petDTO.PetGender.ToLower();
            if (petGender != maleGender.ToLower() && petGender != femaleGender.ToLower())
            {
                return APIResponse<Pet>.CreateResponse(false, WrongGenderMessage, null);
            }
            return APIResponse<Pet>.CreateResponse(true, userId, null);
        }

        public async Task<APIResponse<IEnumerable<Pet>>> GetPetsByAge(int age)
        {
            try
            {
                if (age <= 0) return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.InvalidId, null);
                var result = await _petRepository.GetPetsByAge(age);
                if (result == null || !result.Any()) return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.DataNotFound, null);
                return APIResponse<IEnumerable<Pet>>.CreateResponse(true, MessageHelper.Success(nameof(Pet), MessageConstants.fetchedMessage), result);
            }
            catch (Exception ex)
            {
                return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageHelper.Exception(nameof(Pet), MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<IEnumerable<Pet>>> GetPetsByAgeRange(int minAge, int maxAge)
        {
            try
            {
                if (minAge < 0 || maxAge <= 0)
                {
                    return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.InvalidId, null);
                }
                var result = await _petRepository.GetPetsByAgeRange(minAge, maxAge);
                if (result == null || !result.Any()) return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.DataNotFound, null);
                return APIResponse<IEnumerable<Pet>>.CreateResponse(true, MessageHelper.Success(nameof(Pet), MessageConstants.fetchedMessage), result);
            }
            catch (Exception ex)
            {
                return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageHelper.Exception(nameof(Pet), MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<IEnumerable<Pet>>> GetPetsByBreedId(int id)
        {
            try
            {
                if (id <= 0) return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.InvalidId, null);

                var breedExists = await _breedRepository.GetById(id);
                if (breedExists == null) return APIResponse<IEnumerable<Pet>>.CreateResponse(false, NotExistsBreedMessage, null);

                var result = await _petRepository.GetPetsByBreedId(id);
                if (result == null || !result.Any()) return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.DataNotFound, null);
                return APIResponse<IEnumerable<Pet>>.CreateResponse(true, MessageHelper.Success(nameof(Pet), MessageConstants.fetchedMessage), result);
            }
            catch (Exception ex)
            {
                return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageHelper.Exception(nameof(Pet), MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<IEnumerable<Pet>>> GetPetsByGender(string gender)
        {
            try
            {
                if (!gender.IsNullOrEmpty() && gender.ToLower() == maleGender.ToLower() || gender.ToLower() == femaleGender.ToLower())
                {
                    var result = await _petRepository.GetPetsByGender(gender);
                    if (result == null || !result.Any()) return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.DataNotFound, null);
                    return APIResponse<IEnumerable<Pet>>.CreateResponse(true, MessageHelper.Success(nameof(Pet), MessageConstants.fetchedMessage), result);
                }

                return APIResponse<IEnumerable<Pet>>.CreateResponse(false, WrongGenderMessage, null);
            }
            catch (Exception ex)
            {
                return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageHelper.Exception(nameof(Pet), MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<IEnumerable<Pet>>> GetYourPets()
        {
            try
            {
                var currentUserId = _httpContextHelper.GetCurrentUserId();
                if (currentUserId == null) return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.UnAuthenticatedUserMessage, null);

                var result = await _petRepository.GetYourPets(currentUserId);
                if (result == null || !result.Any()) return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.DataNotFound, null);
                return APIResponse<IEnumerable<Pet>>.CreateResponse(true, MessageHelper.Success(nameof(Pet), MessageConstants.fetchedMessage), result);
            }
            catch (Exception ex)
            {
                return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageHelper.Exception(nameof(Pet), MessageConstants.fetchingMessage, ex.Message), null);
            }
        }
    }
}