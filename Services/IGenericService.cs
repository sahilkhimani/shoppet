using shoppetApi.Helper;

namespace shoppetApi.Services
{
    public interface IGenericService<T> where T : class 
    {
        Task<APIResponse<T>> Add(T entity);
        Task<APIResponse<T>> Update(object id,T entity);
        Task<APIResponse<T>> Delete(object id);
        Task<APIResponse<T>> GetById(object id);
        Task<APIResponse<IEnumerable<T>>> GetAll();
        public string ApplyTitleCase(string name);

    }
}
