using NuGet.DependencyResolver;
using PetShopApi.Models;
using shoppetApi.DTO;
using shoppetApi.Helper;
using shoppetApi.Interfaces;
using shoppetApi.MyUnitOfWork;

namespace shoppetApi.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<T> _genericRepository;

        public GenericService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _genericRepository = _unitOfWork.GenericRepository<T>();
      
        }

        public async Task<APIResponse<T>> Add(T entity)
        {
            try
            {
                await _genericRepository.Add(entity);
                await _unitOfWork.SaveAsync();
                return APIResponse<T>.CreateResponse(true, MessageHelper.Success(typeof(T).Name, "created"), entity);
            }
            catch (Exception ex) {
                return APIResponse<T>.CreateResponse(false, MessageHelper.Exception(typeof(T).Name, "creating", ex.Message), null);
            }
        }

        public async Task<APIResponse<T>> Delete(object id)
        {
            try
            {
                if (id == null) return APIResponse<T>.CreateResponse(false, MessageConstants.NullId, null);
                var result = await _genericRepository.GetById(id);
                if (result == null)
                {
                    return APIResponse<T>.CreateResponse(false, MessageHelper.NotFound(typeof(T).Name), null);
                }
                await _genericRepository.Delete(id);
                await _unitOfWork.SaveAsync();
                return APIResponse<T>.CreateResponse(true, MessageHelper.Success(typeof(T).Name, "deleted"), null);
            }
            catch (Exception ex) {
                return APIResponse<T>.CreateResponse(false, MessageHelper.Exception(typeof(T).Name, "deleting", ex.Message), null);
            }
        }

        public async Task<APIResponse<IEnumerable<T>>> GetAll()
        {
            try
            {
                var result = await _genericRepository.GetAll();
                if (result == null)
                {
                    return APIResponse<IEnumerable<T>>.CreateResponse(false, MessageHelper.NotFound(typeof(T).Name), null);
                }
                return APIResponse<IEnumerable<T>>.CreateResponse(true, MessageHelper.Success(typeof(T).Name, "fetched"), result);
            }
            catch (Exception ex)
            {
                return APIResponse<IEnumerable<T>>.CreateResponse(false, MessageHelper.Exception(typeof(T).Name, "fetching", ex.Message), null);
            }
        }

        public async Task<APIResponse<T>> GetById(object id)
        {
            try
            {
                if(id == null) return APIResponse<T>.CreateResponse(false, MessageConstants.NullId, null);
               
                var result = await _genericRepository.GetById(id);
                
                if (result == null)
                {
                    return APIResponse<T>.CreateResponse(false, MessageHelper.NotFound(typeof(T).Name), null);
                }
                return APIResponse<T>.CreateResponse(true, MessageHelper.Success(typeof(T).Name, "retrieved"), result);
            }
            catch (Exception ex)
            {
                return APIResponse<T>.CreateResponse(false, MessageHelper.Exception(typeof(T).Name, "retrieving", ex.Message), null);
            }

        }   

        public async Task<APIResponse<T>> Update(object id, T entity)
        {
            try
            {
                if (id == null) return APIResponse<T>.CreateResponse(false, MessageConstants.NullId, null);

                var data = await _genericRepository.GetById(id);
                if (data == null)
                {
                    return APIResponse<T>.CreateResponse(false, MessageHelper.NotFound(typeof(T).Name), null);
                }
                await _genericRepository.Update(id, entity);
                await _unitOfWork.SaveAsync();
                return APIResponse<T>.CreateResponse(true, MessageHelper.Success(typeof(T).Name, "updated"), null);
   
            }
            catch (Exception ex)
            {
                return APIResponse<T>.CreateResponse(false, MessageHelper.Exception(typeof(T).Name, "updating", ex.Message), null);
            }
        }
    }
}
