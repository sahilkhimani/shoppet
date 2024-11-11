using shoppetApi.Helper;
using shoppetApi.Interfaces;
using shoppetApi.UnitOfWork;

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
                return new APIResponse<T>
                {
                    Success = true,
                    Message = MessageHelper.Success(typeof(T).Name, "created"),
                    Data = entity
                };
            }
            catch (Exception ex) {
                return new APIResponse<T>
                {
                    Success = false,
                    Message = MessageHelper.Exception(typeof(T).Name, "creating", ex.Message),
                };
            }
        }

        public async Task<APIResponse<T>> Delete(int id)
        {
            try
            {
                var result = await _genericRepository.GetById(id);
                if (result == null)
                {
                    return new APIResponse<T>
                    {
                        Success = false,
                        Message = MessageHelper.NotFound(typeof(T).Name)
                    };
                }
                await _genericRepository.Delete(id);
                await _unitOfWork.SaveAsync();
                return new APIResponse<T>
                {
                    Success = true,
                    Message = MessageHelper.Success(typeof(T).Name, "deleted")
                };
            }
            catch (Exception ex) {
                return new APIResponse<T>
                {
                    Success = false,
                    Message = MessageHelper.Exception(typeof(T).Name, "deleting", ex.Message)
                };
            }
        }

        public async Task<APIResponse<IEnumerable<T>>> GetAll()
        {
            try
            {
                var result = await _genericRepository.GetAll();
                if (result == null)
                {
                    return new APIResponse<IEnumerable<T>>()
                    {
                        Success = false,
                        Message = MessageHelper.NotFound(typeof(T).Name),
                    };
                }
                return new APIResponse<IEnumerable<T>>()
                {
                    Success = true,
                    Message = MessageHelper.Success(typeof(T).Name, "fetched"),
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<IEnumerable<T>>
                {
                    Success = false,
                    Message = MessageHelper.Exception(typeof(T).Name, "fetching", ex.Message)
                };
            }
        }

        public async Task<APIResponse<T>> GetById(int id)
        {
            try
            {
                var result = await _genericRepository.GetById(id);
                if (result == null)
                {
                    return new APIResponse<T>
                    {
                        Success = false,
                        Message = MessageHelper.NotFound(typeof(T).Name)
                    };
                }
                return new APIResponse<T> {
                    Success = true,
                    Message = MessageHelper.Success(typeof(T).Name, "retrieved"),
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<T>
                {
                    Success = false,
                    Message = MessageHelper.Exception(typeof(T).Name, "retrieving", ex.Message)
                };
            }

        }

        public async Task<APIResponse<T>> Update(int id, T entity)
        {
            try
            {
                var data = await _genericRepository.GetById(id);
                if(data == null)
                {
                    return new APIResponse<T>
                    {
                        Success = false,
                        Message = MessageHelper.NotFound(typeof(T).Name)
                    };
                }

                await _genericRepository.Update(id, entity);
                await _unitOfWork.SaveAsync();
                return new APIResponse<T>
                {
                    Success = true,
                    Message = MessageHelper.Success(typeof(T).Name, "updated")
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<T>
                {
                    Success = false,
                    Message = MessageHelper.Exception(typeof(T).Name, "updating", ex.Message)
                };
            }
        }
    }
}
