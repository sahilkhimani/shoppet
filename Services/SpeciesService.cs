using AutoMapper;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Interfaces;
using shoppetApi.MyUnitOfWork;

namespace shoppetApi.Services
{
    public class SpeciesService : ISpeciesService
    {
        public const string AlredyExistsSpeciesMessage = "Species already exists";

        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IMapper _mapper;
        public SpeciesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _speciesRepository = unitOfWork.Species;
            _mapper = mapper;
        }
        public async Task<bool> AlreadyExists(string name)
        {
            return await _speciesRepository.SpeciesAlreadyExists(name);
        }
        public async Task<APIResponse<Species>> Add(SpeciesDTO speciesDTO)
        {
            try
            {
                var alreadyExist = await AlreadyExists(speciesDTO.SpeciesName);
                if (alreadyExist) return APIResponse<Species>.CreateResponse(false, AlredyExistsSpeciesMessage, null);

                var data = _mapper.Map<Species>(speciesDTO);
                await _speciesRepository.Add(data);
                await _unitOfWork.SaveAsync();
                return APIResponse<Species>.CreateResponse(true, MessageHelper.Success(nameof(Species), MessageConstants.createdMessage), data);
            }
            catch (Exception ex)
            {
                return APIResponse<Species>.CreateResponse(false, MessageHelper.Exception(nameof(Species), MessageConstants.creatingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<Species>> Update(string id, SpeciesDTO speciesDTO)
        {
            try
            {
                var parsedId = HelperMethods.ParseId(id);
                if (parsedId == null) return APIResponse<Species>.CreateResponse(false, MessageConstants.InvalidId, null);
                var data = await _speciesRepository.GetById(parsedId);
                if (data == null) return APIResponse<Species>.CreateResponse(false, MessageHelper.NotFound(nameof(Species)), null);

                var alreadyExist = await AlreadyExists(speciesDTO.SpeciesName);
                if (alreadyExist) return APIResponse<Species>.CreateResponse(false, AlredyExistsSpeciesMessage, null);

                var updatedData = _mapper.Map(speciesDTO, data);

                await _speciesRepository.Update(parsedId, data);
                await _unitOfWork.SaveAsync();
                return APIResponse<Species>.CreateResponse(true, MessageHelper.Success(nameof(Species), MessageConstants.updatedMessage), data);
            }
            catch (Exception ex)
            {
                return APIResponse<Species>.CreateResponse(false, MessageHelper.Exception(nameof(Species), MessageConstants.updatingMessage, ex.Message), null);
            }
        }
    }
}
