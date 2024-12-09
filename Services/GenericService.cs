using AutoMapper;
using shoppetApi.Helper;
using shoppetApi.Interfaces;
using shoppetApi.MyUnitOfWork;

namespace shoppetApi.Services
{
    public class GenericService<T, TAdd, TUpdate> : IGenericService<T, TAdd, TUpdate> where T : class where TAdd : class where TUpdate : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<T> _genericRepository;
        private readonly IMapper _mapper;

        public GenericService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = _unitOfWork.GenericRepository<T>();
            _mapper = mapper;
        }

        public async Task<APIResponse<T>> Add(TAdd dto)
        {
            try
            {
                var data = _mapper.Map<T>(dto);
                await _genericRepository.Add(data);
                await _unitOfWork.SaveAsync();
                return APIResponse<T>.CreateResponse(true, MessageHelper.Success(typeof(T).Name, MessageConstants.createdMessage), data);
            }
            catch (Exception ex)
            {
                return APIResponse<T>.CreateResponse(false, MessageHelper.Exception(typeof(T).Name, MessageConstants.creatingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<T>> Delete(object id)
        {
            try
            {
                var result = await GetById(id);
                if (!result.Success) return result;

                var parsedId = HelperMethods.ParseId(id);
                await _genericRepository.Delete(parsedId!);
                await _unitOfWork.SaveAsync();
                return APIResponse<T>.CreateResponse(true, MessageHelper.Success(typeof(T).Name, MessageConstants.deletedMessage), null);
            }
            catch (Exception ex)
            {
                return APIResponse<T>.CreateResponse(false, MessageHelper.Exception(typeof(T).Name, MessageConstants.deletingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<IEnumerable<T>>> GetAll()
        {
            try
            {
                var result = await _genericRepository.GetAll();
                if (!result.Any()) return APIResponse<IEnumerable<T>>.CreateResponse(false, MessageHelper.NotFound(typeof(T).Name), null);

                return APIResponse<IEnumerable<T>>.CreateResponse(true, MessageHelper.Success(typeof(T).Name, MessageConstants.fetchedMessage), result);
            }
            catch (Exception ex)
            {
                return APIResponse<IEnumerable<T>>.CreateResponse(false, MessageHelper.Exception(typeof(T).Name, MessageConstants.fetchingMessage, ex.Message), null);
            }
        }

        public async Task<APIResponse<T>> GetById(object id)
        {
            try
            {
                var parsedId = HelperMethods.ParseId(id);
                if (parsedId == null) return APIResponse<T>.CreateResponse(false, MessageConstants.InvalidId, null);

                var result = await _genericRepository.GetById(parsedId);
                if (result == null) return APIResponse<T>.CreateResponse(false, MessageHelper.NotFound(typeof(T).Name), null);

                return APIResponse<T>.CreateResponse(true, MessageHelper.Success(typeof(T).Name, MessageConstants.fetchedMessage), result);
            }
            catch (Exception ex)
            {
                return APIResponse<T>.CreateResponse(false, MessageHelper.Exception(typeof(T).Name, MessageConstants.fetchingMessage, ex.Message), null);
            }

        }

        public async Task<APIResponse<T>> Update(object id, TUpdate dto)
        {
            try
            {
                var data = await GetById(id);
                if (!data.Success) return data;

                var updatedData = _mapper.Map(dto, data.Data);

                await _genericRepository.Update(id, updatedData!);
                await _unitOfWork.SaveAsync();

                return APIResponse<T>.CreateResponse(true, MessageHelper.Success(typeof(T).Name, MessageConstants.updatedMessage), null);

            }
            catch (Exception ex)
            {
                return APIResponse<T>.CreateResponse(false, MessageHelper.Exception(typeof(T).Name, MessageConstants.updatingMessage, ex.Message), null);
            }
        }
    }
}
