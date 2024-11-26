using Microsoft.Identity.Client;
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
        public string maleGender = "Male";
        public string femaleGender = "Female";
        public PetService(IUnitOfWork unitOfWork, IHttpContextHelper httpContextHelper)
        {
            _unitOfWork = unitOfWork;
            _breedRepository = _unitOfWork.Breeds;
            _httpContextHelper = httpContextHelper;
            _petRepository = _unitOfWork.Pets;
        }
        public async Task<bool> BreedIdAlreadyExists(int id)
        {
            return await _breedRepository.BreedIdAlreadyExists(id);
        }

        public async Task<string> CheckOwnPets(string id)
        {
            int parsedId = 0;
            if (int.TryParse(id, out var intId))
            {
                if (intId <= 0)
                {
                    return MessageConstants.InvalidId;
                }
                parsedId = intId;
            }
            var existId = await _petRepository.GetById(parsedId);
            if (existId == null)
            {
                return MessageConstants.DataNotFound;
            }
            var ownerId = _petRepository.GetOwnerOnPetId(parsedId);
            var currentUserId = _httpContextHelper.GetCurrentUserId();
            if (ownerId == null || ownerId != currentUserId)
            {
                return MessageConstants.UnAuthorizedUser;
            }
            return ownerId;
        }

        public async Task<string> CheckUser(PetDTO petDTO)
        {
            var userId = _httpContextHelper.GetCurrentUserId();
            var breedExists = await BreedIdAlreadyExists(petDTO.BreedId);
            var petGender = petDTO.PetGender.ToLower();
            if (userId == null)
            {
                return MessageConstants.NoUser;
            }
            if (!breedExists)
            {
                return MessageConstants.NotExistsBreed;
            }
            if (petGender != maleGender.ToLower() && petGender != femaleGender.ToLower())
            {
                return MessageConstants.WrongGender;
            }
            return userId;
        }

        public async Task<APIResponse<IEnumerable<Pet>>> GetPetsByAge(int age)
        {
            if (age <= 0)
            {
                return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.InvalidId, null);
            }
            var result = await _petRepository.GetPetsByAge(age);
            if (result == null || !result.Any())
            {
                return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.DataNotFound, null);
            }
            return APIResponse<IEnumerable<Pet>>.CreateResponse(true, MessageHelper.Success(typeof(Pet).Name, MessageConstants.fetchedMessage), result);
        }

        public async Task<APIResponse<IEnumerable<Pet>>> GetPetsByAgeRange(int minAge, int maxAge)
        {
            if (minAge <= 0 || maxAge <= 0)
            {
                return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.InvalidId, null);
            }
            var result = await _petRepository.GetPetsByAgeRange(minAge, maxAge);
            if (result == null || !result.Any())
            {
                return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.DataNotFound, null);
            }
            return APIResponse<IEnumerable<Pet>>.CreateResponse(true, MessageHelper.Success(typeof(Pet).Name, MessageConstants.fetchedMessage), result);

        }

        public async Task<APIResponse<IEnumerable<Pet>>> GetPetsByBreedId(int id)
        {
            if (id <= 0)
            {
                return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.InvalidId, null);
            }
            var breedExists = await _breedRepository.GetById(id);
            if(breedExists == null) return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.DataNotFound, null);

            var result = await _petRepository.GetPetsByBreedId(id);
            return APIResponse<IEnumerable<Pet>>.CreateResponse(true, MessageHelper.Success(typeof(Pet).Name, "fetched"), result);
        }

        public async Task<APIResponse<IEnumerable<Pet>>> GetPetsByGender(string gender)
        {
            if(!gender.IsNullOrEmpty() && gender.ToLower() == "male" || gender.ToLower() == "female")
            {
                var result = await _petRepository.GetPetsByGender(gender);
                if (result == null || !result.Any())
                {
                    return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.DataNotFound, null);
                }
                return APIResponse<IEnumerable<Pet>>.CreateResponse(true, MessageHelper.Success(typeof(Pet).Name, MessageConstants.fetchedMessage), result);

            }
            return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.WrongGender, null);
        }

        public async Task<APIResponse<IEnumerable<Pet>>> GetYourPets()
        {
            var currentUserId = _httpContextHelper.GetCurrentUserId();
            if(currentUserId == null)
            {
                return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.UnAuthenticatedUser, null);
            }
            var result = await _petRepository.GetYourPets(currentUserId);
            if(result == null || !result.Any())
            {
                return APIResponse<IEnumerable<Pet>>.CreateResponse(false, MessageConstants.DataNotFound, null) ;   
            }
            return APIResponse<IEnumerable<Pet>>.CreateResponse(true, MessageHelper.Success(typeof(Pet).Name, MessageConstants.fetchedMessage), result);
        }
    }
}
