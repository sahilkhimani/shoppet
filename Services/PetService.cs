using Microsoft.Identity.Client;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Interfaces;

namespace shoppetApi.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;
        private readonly IBreedRepository _breedRepository;
        private readonly IHttpContextHelper _httpContextHelper;
        public string maleGender = "Male";
        public string femaleGender = "Female";
        public PetService(IBreedRepository breedRepository, IHttpContextHelper httpContextHelper,
            IPetRepository petRepository
            )
        {
            _breedRepository = breedRepository;
            _httpContextHelper = httpContextHelper;
            _petRepository = petRepository;
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
            if(existId == null)
            {
                return MessageConstants.DataNotFound;
            }
            var ownerId = _petRepository.GetOwnerOnPetId(parsedId);
            var currentUserId = _httpContextHelper.GetCurrentUserId();
            if(ownerId == null || ownerId != currentUserId)
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
            if(userId == null)
            {
                return MessageConstants.NoUser;
            }
            if (!breedExists)
            {
                return MessageConstants.NotExistsBreed;
            }
            if(petGender != maleGender.ToLower() && petGender != femaleGender.ToLower())
            {
                return MessageConstants.WrongGender;
            }
            return userId;
        }
    }
}
