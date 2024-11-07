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
                //await _genericRepository.Add(entity);
                //await _unitOfWork.SaveAsync();
                return new APIResponse<T>
                {
                    Success = true,
                    Message = MessageHelper.Success($"{typeof(T).Name}", "created"),
                    Data = entity
                };
            }
            catch (Exception ex) {
                return new APIResponse<T>
                {
                    Success = false,
                    Message = MessageHelper.Exception($"{typeof(T).Name}", "creating", ex.Message),
                };
            }
        }

        public async Task<APIResponse<T>> Delete(int id)
        {
            try
            {
                var entity = _genericRepository.GetById(id);
                if (entity == null)
                {
                    return new APIResponse<T>
                    {
                        Success = false,
                        Message = MessageHelper.NotFound($"{typeof(T).Name}")
                    };
                }
                return entity;
            }
        }

    public Task<APIResponse<IEnumerable<T>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<APIResponse<T>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<APIResponse<T>> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
