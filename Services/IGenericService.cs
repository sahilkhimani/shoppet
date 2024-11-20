using shoppetApi.Helper;

namespace shoppetApi.Services
{
    public interface IGenericService<T> where T : class 
    {
        Task<APIResponse<T>> Add(T entity);
        Task<APIResponse<T>> Update(int id,T entity);
        Task<APIResponse<T>> Delete(int id);
        Task<APIResponse<T>> GetById(int id);
        Task<APIResponse<IEnumerable<T>>> GetAll();
    }
}
